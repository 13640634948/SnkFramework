using System.Collections.Generic;

namespace SnkFramework.PatchBuilder.Runtime.Base
{
    /// <summary>
    /// 资源信息
    /// </summary>
    [System.Serializable]
    public class SourceInfo
    {
        /// <summary>
        /// 资源名（相对路径）
        /// </summary>
        public string name;
        
        /// <summary>
        /// 资源大小
        /// </summary>
        public long size;
        
        /// <summary>
        /// 资源版本
        /// </summary>
        public int version;
        
        /// <summary>
        /// 资源MD5
        /// </summary>
        public string md5;

        /// <summary>
        /// 从字符串解析出资源信息对象（SourceInfo）
        /// </summary>
        /// <param name="content">资源对象的序列化字符串</param>
        /// <returns>资源对象</returns>
        public static SourceInfo Parse(string content)
        {
            var strings = content.Trim().Split(",");
            if (strings.Length != 4)
                throw new System.Exception("解析SourceInfo失败. content:" + content);

            var sourceInfo = new SourceInfo();
            sourceInfo.name = strings[0];
            sourceInfo.size = long.Parse(strings[1]);
            sourceInfo.version = int.Parse(strings[2]);
            sourceInfo.md5 = strings[3];
            return sourceInfo;
        }

        /// <summary>
        /// 数据对象序列化（SourceInfo）
        /// </summary>
        /// <returns>序列化后的字符串</returns>
        public string ToSerializable()
            => string.Format($"{name},{size},{version},{md5}");
    }
    
    /// <summary>
    /// 资源信息比较器
    /// </summary>
    public class SourceInfoComparer : IEqualityComparer<SourceInfo>
    {
        public bool Equals(SourceInfo x, SourceInfo y)
            => y != null && x != null && x.name == y.name && x.md5 == y.md5;
        
        public int GetHashCode(SourceInfo obj) => obj.ToString().GetHashCode();
    }
}