using System.Threading.Tasks;

namespace SnkFramework.CloudRepository.Runtime
{
    public interface ISnkCloudRepository
    {
        public bool Preview(string key);
        
        public Task<bool> PreviewAsync(string key);

        public bool Apply(string key);
        
        public Task<bool> ApplyAsync(string key);
    }
}