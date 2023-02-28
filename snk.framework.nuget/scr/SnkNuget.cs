using System.Collections.Generic;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Exceptions;

namespace SnkFramework.NuGet
{
    public class SnkNuget
    {
        public static ISnkCodeGenerator CodeGenerator = new SnkMD5Generator();
    }
}

