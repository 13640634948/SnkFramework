using System.Threading.Tasks;
using BFFramework.Runtime.Services;
using BFFramework.Runtime.UserInterface;
using Cysharp.Threading.Tasks;
using GAME.Contents.UserInterfaces.ViewModels;
using Loxodon.Framework.Binding;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace GAME.Contents.UserInterfaces
{
    namespace ViewModels
    {
        public class ProgressBar : BFViewModel
        {
            private float progress;
            private string tip;
            private bool enable;

            public bool Enable
            {
                get => this.enable;
                set => this.Set<bool>(ref this.enable, value);
            }

            public float Progress
            {
                get => this.progress;
                set => this.Set<float>(ref this.progress, value);
            }

            public string Tip
            {
                get => this.tip;
                set => this.Set<string>(ref this.tip, value);
            }
        }

        public class PatchViewModel : BFViewModel
        {
            private ProgressBar _progressBar = new ProgressBar();

            public ProgressBar ProgressBar => this._progressBar;

            private IBFPatchService _patchService;
            protected IBFPatchService patchService => this._patchService ??= Snk.IoCProvider.Resolve<IBFPatchService>();


            public async Task ExePatch()
            {
                try
                {
                    var needPatch = await patchService.IsNeedPatch();
                    SnkLogHost.Default?.Info("PatchService.IsNeedPatch:" + needPatch);
                    if (needPatch == false)
                    {
                        Debug.Log("ExePatch => return");
                        return;
                    }
                    patchService.Apply();
                    while (patchService.IsDone == false)
                    {
                        this.ProgressBar.Progress = patchService.Progress;
                        Debug.Log($"[PatchWindow]progress:{patchService.Progress}, {patchService.IsDone}");
                        await new WaitForSecondsRealtime(0.02f);
                    }

                    this.ProgressBar.Progress = 1.0f;
                    Debug.Log("patchService.Progress-last:" + patchService.Progress + " - " + patchService.IsDone);
                }
                finally
                {
                    //this.command.Enabled = true;
                    //this.progressBar.Enable = false;
                    //this.progressBar.Tip = "";
                    //this.command.Execute(null);
                }
            }
        }
    }

    namespace Views
    {
        public class PatchWindow : BFWindow
        {
            public Slider downloadSlider;
            public Text progressText;
            public Text tipText;

            protected override void onCreate(ISnkBundle bundle = null)
            {
                base.onCreate(bundle);
                var viewModel = new PatchViewModel();
                var bindingSet = this.CreateBindingSet(viewModel);

                bindingSet.Bind(this.tipText).For(v => v.text).To(vm => vm.ProgressBar.Progress).OneWay();
                bindingSet.Bind(this.downloadSlider).For(v => v.value).To(vm => vm.ProgressBar.Progress).OneWay();
                bindingSet.Build();

                viewModel.ExePatch();
            }
        }
    }
}