using System.Threading.Tasks;

namespace BFFramework.Runtime.Services
{
    public interface IBFPatchService
    {
          float Progress { get;}
          bool IsDone { get; }
          
          Task Initialize();

          Task<bool> IsNeedPatch();
              
          Task Apply();

    }
}