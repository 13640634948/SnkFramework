namespace SnkFramework.PatchBuilder.Runtime
{
    public class SNK_BUILDER_CONST
    {
        /* 压缩模式 */
        public const bool COMPRESS_MODE = false;
        
        /* 压缩文件后缀 */
        public const string COMPRESS_FILE_SUFFIX = ".zip";
        
        /* 补丁包仓库路径 */
        public const string REPO_ROOT_PATH = "PatcherRepository";

        /* 设置文件文件名 */
        public const string SETTING_FILE_NAME = ".setting.json";
        
        /* 资源清单文件名 */
        public const string SOURCE_FILE_NAME = "source_manifest.json";
        
        /* 差异清单文件名 */
        public const string DIFF_FILE_NAME = ".diff_manifest.json";
        
        /* 版本信息文件 */
        public const string VERSION_INFO_FILE_NAME = "version_info.json";
        
        /* 版本资源目录格式 */
        public const string VERSION_DIR_NAME_FORMATER = "version_{0}_{1}_b{2}";

        /* 版本资源中间目录 */
        public const string VERSION_SOURCE_MID_DIR_PATH = "source_files";

    }
}