using System.Collections;

namespace SampleDevelop.Test
{
    public enum LoadState
    {
        none,
        load_begin,
        load_end,
        unload_begin,
        unload_end
    }
    
    public interface ISnkLoader
    {
        public LoadState mLoadState { get; }
        public string mAssetPath { get; }
        public void Load();
        public IEnumerator LoadAsync();

        public void Unload();
    }
}