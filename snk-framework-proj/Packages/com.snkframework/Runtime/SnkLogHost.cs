namespace DefaultNamespace
{
    
    /*
    //public static ISnkJsonParser JsonParser;
    //public static ISnkCodeGenerator CodeGenerator;

    private static ISnkSetup s_Setup;

    //public static IMvxIoCProvider IoCProvider => SnkSingleton<IMvxIoCProvider>.Instance;

    public static void Launcher<TSetup>() where TSetup : class, ISnkSetup, new()
    {
        s_Setup = new TSetup();
        s_Setup.InitializePrimary();
        s_Setup.InitializeSecondary();
    }
    */
    
    /*
     
    internal class SnkLogHost
    {
        private static ISnkLogger _defaultLogger;

        public static ISnkLogger Default
        {
            get
            {
                if (_defaultLogger == null)
                    _defaultLogger = GetLog("default");
                return _defaultLogger;
            }
        }

        public static ISnkLogger GetLog(string categoryName)
        {
            if (Snk.IoCProvider.TryResolve<ISnkLoggerFactory>(out var loggerFactory))
                return loggerFactory.CreateLogger(categoryName);
            return null;
        }

        public static ISnkLogger GetLog<T>() => GetLog(typeof(T).FullName);
    }
    */
}