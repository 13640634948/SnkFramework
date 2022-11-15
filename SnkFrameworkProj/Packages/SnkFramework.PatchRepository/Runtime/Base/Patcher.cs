namespace SnkFramework.PatchRepository.Runtime.Base
{
    [System.Serializable]
    public class Patcher
    {
        public string name;
        public int version;

        public int sourceCount;
        public long totalSize;

        public bool force;
        public bool compress;
    }
}