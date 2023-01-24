using SnkFramework.NuGet.Basic;

namespace SnkFramework.NuGet
{
    namespace Core
    {
        public interface ISnkSetup
        {
            ISnkJsonParser CreateJsonParser();

            ISnkCompressor CreateCompressor();

            ISnkCodeGenerator CreateCodeGenerator();

            ISnkLogger CreateLogger();
        }
    }
}