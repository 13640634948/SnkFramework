using SnkFramework.Runtime;

namespace GAME.Contents
{
    public class Game : Snk
    {
        public static TService ResolveService<TService>() where TService: class
            => IoCProvider.Resolve<TService>();

        public static T Resolve<T>() where T : class
            => IoCProvider.Resolve<T>();
    }
}