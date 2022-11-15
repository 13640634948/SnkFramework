using SnkFramework.PatchRepository.Runtime;
using SnkFramework.PatchRepository.Runtime.Base;
using UnityEngine;

namespace SnkFramework.PatchRepository
{
    namespace Demo
    {
        public class RepositoryDemo : MonoBehaviour
        {
            private string repoName = "windf_iOS";
            private Repository sourceRepo;

            void Start()
            {
                var sourcePaths = new SourcePath[]
                {
                    new() { fullPath = "Temp" },
                };

                sourceRepo = Repository.Load(repoName);
                var patcher = sourceRepo.Build(sourcePaths);
            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }
}