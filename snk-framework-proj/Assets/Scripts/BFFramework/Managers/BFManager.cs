using SnkFramework.NuGet.Basic;

namespace BFFramework.Runtime.Managers
{
    public abstract class BFManager<TManager> : SnkSingleton<TManager>, IBFManager
        where TManager : class, IBFManager
    {
        
    }
}