using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.NuGet.Exceptions;

namespace SnkFramework.Runtime.Basic
{
    namespace FiniteStateMachine
    {
        public class SnkFiniteStateMachine<TFSMOwner> where TFSMOwner : class, ISnkFiniteStateMachineOwner
        {
            private struct StateCachePair
            {
                public string stateName;
                public ISnkState<TFSMOwner> instance;
                public object userData;
            }
            
            private static readonly object _locker = new object();

            private readonly TFSMOwner _owner;
            private readonly Dictionary<string, ISnkState<TFSMOwner>> _stateDict;
            private readonly Queue<StateCachePair> _stateQue;

            private ISnkState<TFSMOwner> _currState;
            private string _currStateName;
            private TaskCompletionSource<bool> _taskCompletionSource;
            private StateCachePair _tmpStateCachePair;

            public SnkFiniteStateMachine(TFSMOwner owner)
            {
                this._owner = owner;
                this._stateDict = new Dictionary<string, ISnkState<TFSMOwner>>();
                this._stateQue = new Queue<StateCachePair>();
            }

            public void RegisterState<TState>(object userData = null, string stateName = null)
                where TState : ISnkState<TFSMOwner>, new()
            {
                var state = new TState();
                state.Initialize(userData);
                this._stateDict.Add(stateName ?? typeof(TState).Name, state);
            }

            private bool ValidateTaskCompletionSource()
            {
                if (_taskCompletionSource != null && _taskCompletionSource.Task.IsCompleted == false)
                    return true;
                _taskCompletionSource = new TaskCompletionSource<bool>();
                return false;
            }

            public async Task Switch(string stateName, object userData, bool enqueueQueue = true)
            {
                if (this._stateDict.TryGetValue(stateName, out var nextState) == false)
                    throw new SnkException("没有找到状态：" + stateName);

                lock (_locker)
                {
                    _stateQue.Enqueue(new StateCachePair()
                    {
                        stateName = stateName,
                        instance = nextState,
                        userData = userData
                    });
                    
                    if (enqueueQueue == false)
                    {
                        while (_stateQue.Count > 1)
                        {
                            _stateQue.TryDequeue(out _);
                        }
                    }
                }

                if (ValidateTaskCompletionSource() == false)
                    SwitchNextState();

                await _taskCompletionSource.Task.ConfigureAwait(false);
            }

            private async Task SwitchNextState()
            {
                while (true)
                {
                    lock (_locker)
                    {
                        if (this._stateQue.TryDequeue(out _tmpStateCachePair) == false)
                        {
                            _taskCompletionSource.SetResult(true);
                            break;
                        }
                    }

                    if (this._currState != null)
                        await this._currState.OnExit(_owner, _tmpStateCachePair.stateName);
                    await _tmpStateCachePair.instance.OnEnter(_owner, _tmpStateCachePair.userData, _currStateName);
                    this._currState = _tmpStateCachePair.instance;
                    this._currStateName = _tmpStateCachePair.stateName;
                }
            }
        }
    }
}