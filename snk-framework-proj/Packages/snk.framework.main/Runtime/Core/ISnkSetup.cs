using System.Threading.Tasks;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public interface ISnkSetup
        {
            void InitializePrimary();
            Task InitializeSecondary();
        }
    }
}