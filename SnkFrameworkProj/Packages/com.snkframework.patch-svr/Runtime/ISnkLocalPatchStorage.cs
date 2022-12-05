namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkLocalPatchStorage : ISnkPatchStorage
    {
        public string LocalPath { get; }
    }
}