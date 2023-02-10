using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.NuGet.Exceptions;

namespace SnkFramework.Runtime.Basic
{
    namespace FiniteStateMachine
    {
        public class SnkFiniteStateMachine<TFSMOwner> where TFSMOwner : class, ISnkFiniteStateMachineOwner
        {
            private static object _locker = new object();

            private readonly TFSMOwner _owner;
            private readonly Dictionary<string, ISnkState<TFSMOwner>> _stateDict;
            private readonly Queue<Tuple<string, ISnkState<TFSMOwner>, object>> _stateQue;

            private ISnkState<TFSMOwner> _currState;
            private string _currStateName;
            private TaskCompletionSource<bool> _taskCompletionSource;
            private Tuple<string, ISnkState<TFSMOwner>, object> _tmpStateTuple;

            public SnkFiniteStateMachine(TFSMOwner owner)
            {
                this._owner = owner;
                this._stateDict = new Dictionary<string, ISnkState<TFSMOwner>>();
                this._stateQue = new Queue<Tuple<string, ISnkState<TFSMOwner>, object>>();
            }

            public void RegisterState<TState>(object userData = null, string stateName = null)
                where TState : ISnkState<TFSMOwner>, new()
            {
                var state = new TState();
                state.Initialize(userData);
                this._stateDict.Add(stateName ?? typeof(TState).Name, state);
            }

            private bool validateTaskCompletionSource()
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

                var tuple = new Tuple<string, ISnkState<TFSMOwner>, object>(stateName, nextState, userData);
                lock (_locker)
                {
                    _stateQue.Enqueue(tuple);
                    if (enqueueQueue == false)
                    {
                        while (_stateQue.Count > 1)
                        {
                            _stateQue.TryDequeue(out _);
                        }
                    }
                }

                if (validateTaskCompletionSource() == false)
                    switchNextState();

                await _taskCompletionSource.Task.ConfigureAwait(false);
            }

            private async Task switchNextState()
            {
                while (true)
                {
                    lock (_locker)
                    {
                        if (this._stateQue.TryDequeue(out _tmpStateTuple) == false)
                        {
                            _taskCompletionSource.SetResult(true);
                            break;
                        }
                    }

                    if (this._currState != null)
                        await this._currState.OnExit(_owner, _tmpStateTuple.Item1);
                    await _tmpStateTuple.Item2.OnEnter(_owner, _tmpStateTuple.Item3, _currStateName);
                    this._currState = _tmpStateTuple.Item2;
                    this._currStateName = _tmpStateTuple.Item1;
                }
            }
        }
    }
}