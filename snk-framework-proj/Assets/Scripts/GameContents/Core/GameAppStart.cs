using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SnkFramework.Runtime.Core;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GAME.Contents.Core
{
    public class GameAppStart : ISnkAppStart
    {
        public async Task StartAsync(object hint = null)
        {
            await EditorSceneManager.LoadSceneAsync("LoginScene", LoadSceneMode.Additive);
        }

        public bool IsStarted { get; }
        public void ResetStart()
        {
            
        }
    }
}