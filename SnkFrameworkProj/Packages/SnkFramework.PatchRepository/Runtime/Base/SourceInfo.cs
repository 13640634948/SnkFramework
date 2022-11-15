using System.Collections.Generic;

namespace SnkFramework.PatchRepository.Runtime.Base
{
    [System.Serializable]
    public class SourceInfo
    {
        public string name;
        public long size;
        public int version;
        public string md5;

        public static SourceInfo ValueOf(string content)
        {
            string[] datas = content.Trim().Split(",");
            if (datas.Length != 4)
                return null;
            var sourceInfo = new SourceInfo();
            sourceInfo.name = datas[0];
            sourceInfo.size = long.Parse(datas[1]);
            sourceInfo.version = int.Parse(datas[2]);
            sourceInfo.md5 = datas[3];
            return sourceInfo;
        }

        public string ToSerializable()
            => string.Format($"{name},{size},{version},{md5}");
    }
    
    public class SourceInfoComparer : IEqualityComparer<SourceInfo>
    {
        public bool Equals(SourceInfo x, SourceInfo y)
        {
            return y != null && x != null && x.name == y.name && x.md5 == y.md5;
        }

        public int GetHashCode(SourceInfo obj) => obj.ToString().GetHashCode();
    }
}