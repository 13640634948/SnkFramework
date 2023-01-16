using System.IO;

namespace snk.framework.nuget
{
    namespace basic
    {
        public interface ISnkFileFinder
        {
            string SourceDirPath { get; }

            bool TrySurvey(out FileInfo[] fileInfos, out string dirFullPath);
        }
    }
}

