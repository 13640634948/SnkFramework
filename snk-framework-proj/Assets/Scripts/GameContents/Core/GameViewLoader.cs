using System;
using BFFramework.Runtime.UserInterface;

namespace GAME.Contents.Core
{
    public class GameViewLoader : BFViewLoader
    {
        public override Type GetViewType(Type viewModelType)
        {
            return default;
            //return typeof(SplashScreenWindow);
        }
    }
}