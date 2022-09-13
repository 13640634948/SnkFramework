using Loxodon.Framework.Observables;

namespace SnkFramework.FluentBinding.Sample
{
    public class TestViewModel : ObservableObject
    {
        private string tip;

        public string Tip
        {
            get { return this.tip; }
            set { this.Set<string>(ref this.tip, value, nameof(Tip)); }
        }
    }
}