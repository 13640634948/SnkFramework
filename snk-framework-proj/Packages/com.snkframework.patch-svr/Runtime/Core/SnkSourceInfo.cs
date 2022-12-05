namespace SnkFramework.PatchBuilder.Runtime.Core
{
    /// <summary>
    /// 资源信息
    /// </summary>
    [System.Serializable]
    public class SnkSourceInfo
    {
        /// <summary>
        /// 资源名（相对路径）
        /// </summary>
        public string name;

        /// <summary>
        /// 目录名
        /// </summary>
        public string dir;

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
    }
}