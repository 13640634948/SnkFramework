using System.Threading.Tasks;

namespace SnkFramework.Runtime.Core
{
    public interface ISnkAppStart
    {
        //void Start(object? hint = null);

        Task StartAsync(object? hint = null);

        bool IsStarted { get; }

        void ResetStart();
    }

    public class SnkSplashScreenAttribute : System.Attribute
    {
        
    }
    
    public class SnkAppStartAttribute : System.Attribute
    {
        
    }
}