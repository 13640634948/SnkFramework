namespace SnkFramework.PatchBuilder.Runtime.Base
{
    /// <summary>
    /// 资源路径对象
    /// </summary>
    [System.Serializable]
    public class SourcePath
    {
        /// <summary>
        /// 资源目录路径
        /// </summary>
        public string sourceDirPath;
        
        /// <summary>
        /// 筛选关键字
        /// </summary>
        public string[] filters;
        
        /// <summary>
        /// 忽略关键字
        /// </summary>
        public string[] ignores;
    }
}