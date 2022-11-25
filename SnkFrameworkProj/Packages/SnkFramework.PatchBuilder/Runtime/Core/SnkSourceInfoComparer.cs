using System.Collections.Generic;

namespace SnkFramework.PatchBuilder.Runtime.Core
{
    /// <summary>
    /// 资源信息比较器
    /// </summary>
    public class SnkSourceInfoComparer : IEqualityComparer<SnkSourceInfo>
    {
        public bool Equals(SnkSourceInfo x, SnkSourceInfo y)
            => y != null && x != null && x.name == y.name && x.md5 == y.md5;

        public int GetHashCode(SnkSourceInfo obj) => obj.GetType().GetHashCode();
    }
}