using System.IO;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class PatchSettings
    {
        public string repoRootPath;

        public  string channelName;

        public string[] remoteURLArrays;
    }
    
    

    public class SnkLocalSourceRepository : ISnkLocalSourceRepository
    {
        
        public int Version { get; private set; } = 0;
        private PatchSettings _settings;




        public Task Initialize()
        {
            string path = "persistentDataPath/.client";
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                Version = int.Parse(text.Trim());
            }

            return Task.CompletedTask;
        }
        
        public void UpdateLocalResVersion(int resVersion)
        {
            string path = "persistentDataPath/.client";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, resVersion.ToString());
            this.Version = resVersion;
        }

        void ISnkSourceRepository.SetupSettings(PatchSettings settings)
        {
            this._settings = settings;
        }

        public bool Exist(string sourceKey)
        {
            return false;
        }

        public SnkSourceInfo GetSourceInfo(string key)
        {
            throw new System.NotImplementedException();
        }

        public string LocalPath { get; }
    }
}