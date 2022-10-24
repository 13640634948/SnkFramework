using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityViewCreator
    {
        Task<IMvxUnityView> CreateView(MvxViewModelRequest request);
    }
}