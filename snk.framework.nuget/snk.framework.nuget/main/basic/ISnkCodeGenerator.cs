namespace snk.framework.nuget
{
    namespace basic
    {
        public interface ISnkCodeGenerator
        {
            /// <summary>
            /// 文件生成MD5（适用小文件）
            /// </summary>
            /// <param name="filePath">文件路径</param>
            /// <returns>MD5值</returns>
            string GetMD5ByMD5CryptoService(string filePath);

            /// <summary>
            /// 文件生成MD5（适用大文件）
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            string GetMD5ByHashAlgorithm(string filePath);
        }
    }
}

