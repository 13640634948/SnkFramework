namespace SnkFramework.PatchService.Runtime
{
    public class SNK_BUILDER_CONST
    {
        /* 补丁包仓库路径 */
        public const string REPO_ROOT_PATH = "PatcherRepository";

        /* 设置文件文件名 */
        //public const string SETTING_FILE_NAME = ".setting.json";
        
        /* 资源清单文件名 */
        public const string SOURCE_FILE_NAME = "manifest.json";
        
        /* 差异清单文件名 */
        public const string DIFF_FILE_NAME = "diff.json";
        
        /* 版本信息文件 */
        public const string VERSION_INFO_FILE_NAME = "version.json";
        
        /* 版本资源目录格式 */
        //public const string VERSION_DIR_NAME_FORMATER = "version_{0}";

        /* 版本资源中间目录 */
        public const string VERSION_SOURCE_MID_DIR_PATH = "assets";

    }
}