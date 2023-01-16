using snk.framework.nuget.patch;

namespace SnkFramework.Nuget.Sameples
{
    namespace Internals
    {
        internal class SnkSourceInfoComparer : IEqualityComparer<SnkSourceInfo>
        {
            public bool Equals(SnkSourceInfo x, SnkSourceInfo y)
                => x.key == y.key && x.code == y.code;

            public int GetHashCode(SnkSourceInfo obj) => obj.GetType().GetHashCode();
        }
    }
}
