using Loxodon.Framework.Commands;
using Loxodon.Framework.Interactivity;
using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Mvvm
{
    public class LoginViewModel : SnkViewModelBase
    {
        private string tip = "default";

        public string Tip
        {
            get => this.tip;
            set => this.Set(ref this.tip, value, nameof(Tip));
        }

        public SimpleCommand mButtonCommand;

        private InteractionRequest _interactionFinished;
        public IInteractionRequest mInteractionFinished => this._interactionFinished;

        public LoginViewModel()
        {
            this.mButtonCommand = new SimpleCommand(() => log.Info("LoginViewModel - mButtonCommand"));
            this._interactionFinished = new InteractionRequest(this);
        }
    }
}