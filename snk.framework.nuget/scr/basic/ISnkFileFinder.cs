using System.IO;

namespace SnkFramework.NuGet
{
    namespace Basic
    {
        public interface ISnkFileFinder
        {
            string SourceDirPath { get; }

            bool TrySurvey(out FileInfo[] fileInfos);
        }
    }
}

