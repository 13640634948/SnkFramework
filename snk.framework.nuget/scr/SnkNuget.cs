using System.Collections.Generic;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Exceptions;
using SnkFramework.NuGet.Features.Logging;

namespace SnkFramework.NuGet
{
    public class SnkNuget
    {
        public static ISnkLogger Logger;
        public static ISnkCodeGenerator CodeGenerator = new SnkMD5Generator();
    }
}

