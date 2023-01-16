using System.Collections.Generic;

using snk.framework.nuget.basic;
using snk.framework.nuget.patch;

namespace snk.framework.nuget
{
    namespace core
    {
        public interface ISnkSetup
        {
            ISnkJsonParser CreateJsonParser();

            ISnkCompressor CreateCompressor();

            ISnkCodeGenerator CreateCodeGenerator();

            IEqualityComparer<SnkSourceInfo> CreateComparer();
        }
    }
}