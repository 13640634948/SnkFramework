using System.Collections.Generic;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public interface IUIContainer
    {
        List<IView> Views { get; }

        IView GetView(string name);

        void AddView(IView view, bool worldPositionStays = false);

        void AddView(IView view, UILayoutOverride layoutOverride);

        void RemoveView(IView view, bool worldPositionStays = false);
    }
}