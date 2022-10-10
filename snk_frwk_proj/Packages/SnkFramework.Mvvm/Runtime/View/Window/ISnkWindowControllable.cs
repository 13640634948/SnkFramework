using System;

namespace SampleDevelop.Test
{
    public interface ISnkWindowControllable
    {
        public IAsyncResult Activate(bool ignoreAnimation);

        public IAsyncResult Passivate(bool ignoreAnimation);

        public IAsyncResult DoShow(bool ignoreAnimation = false);

        public IAsyncResult DoHide(bool ignoreAnimation = false);

        public void DoDismiss();
    }
}