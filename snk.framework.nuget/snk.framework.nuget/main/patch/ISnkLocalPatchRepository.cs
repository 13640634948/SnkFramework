namespace snk.framework.nuget
{
    namespace patch
    {
        public interface ISnkLocalPatchRepository : ISnkPatchRepository
        {
            string LocalPath { get; }
            void UpdateLocalResVersion(int resVersion);
        }
    }
}