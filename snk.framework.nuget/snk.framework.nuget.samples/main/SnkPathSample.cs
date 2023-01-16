using snk.framework.nuget;
using snk.framework.nuget.basic;
using snk.framework.nuget.patch;

namespace SnkFramework.Nuget.Sameples
{
    internal class SnkPathSample
    {
        private const string unityProjPath = "../../../../../snk-framework-proj/";

        private const string projPath = unityProjPath + "PatcherRepository";
        private const string channelName = "windf_ios";
        private const string appVersion = "1.0.0";

        private static void Main(string[] args)
        {
            Console.WriteLine("currPath:" + Environment.CurrentDirectory);
            try
            {
                Snk.Set<ISnkJsonParser>(new SnkJsonParser());
                Snk.Set<IEqualityComparer<SnkSourceInfo>>(new SnkSourceInfoComparer());
                Snk.Set<ISnkCodeGenerator>(new SnkCodeGenerator());

                var settings = new SnkPatchSettings();
                var builder = SnkPatch.CreatePatchBuilder(projPath, channelName, appVersion, settings);

                var sourcePaths = new SnkFileFinder[]
                {
                new (unityProjPath + "ProjectSettingsDemo")
                {
                    //filters = new [] {"FSTimeGet"},
                    ignores = new [] {".DS_Store"},
                },
                };

                builder.Build(new List<ISnkFileFinder>(sourcePaths));
            }
            catch (Exception e)
            {
                Console.WriteLine("[Exception]" + e.Message + "\n" + e.StackTrace);
                Console.ReadLine();
            }
        }
    }

}