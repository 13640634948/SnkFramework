using SnkFramework.Mvvm.ViewModel;

namespace Windows.LoginWindow
{
    public class LoginViewModel : ViewModel
    {
        private string tip = "default";
        public string Tip
        {
            get => this.tip;
            set => this.Set(ref this.tip, value, nameof(Tip));
        }
    }
}