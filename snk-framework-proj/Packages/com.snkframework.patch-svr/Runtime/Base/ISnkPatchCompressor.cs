using System.IO.Compression;

namespace SnkFramework.PatchBuilder.Runtime.Base
{
    /// <summary>
    /// 压缩器接口
    /// </summary>
    public interface ISnkPatchCompressor
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="dirPath">目录全路径</param>
        /// <param name="zipPath">压缩文件路径</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="includeBaseDirectory">是否包含基础目录</param>
        public void Compress(string dirPath, string zipPath, CompressionLevel compressionLevel, bool includeBaseDirectory);

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipPath">压缩文件路径</param>
        /// <param name="dirPath">目录全路径</param>
        public void Decompress(string zipPath, string dirPath);

        //public Task<bool> CompressAsync(string dirFullPath, string zipFullPath);
        //public Task<bool> DecompressASync(string zipFullPath, string dirFullPath);
    }
}