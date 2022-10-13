using System.Collections;
using SnkFramework.Mvvm.Core;
using SnkFramework.Mvvm.Core.Log;
using SnkFramework.Mvvm.Core.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace UGUI
    {
        public class AlphaAnimation : SnkUIAnimation
        {
            protected static readonly IMvvmLog log = SnkMvvmSetup.mMvvmLog;

            [Range(0f, 1f)] public float from = 1f;
            [Range(0f, 1f)] public float to = 1f;

            public float duration = 2f;

            private IUGUIWindow _window;

            public override void Initialize(ISnkView view)
            {
                base.Initialize(view);

                this._window = view as IUGUIWindow;
                if (_window == null)
                {
                    log.Error("View 类型错误");
                    return;
                }


                if (this.AnimType == ANIM_TYPE.activation_anim ||
                    this.AnimType == ANIM_TYPE.enter_anim)
                {
                    this._window.mAlpha = from;
                }
            }

            protected override IEnumerator DoPlay()
            {
                this.OnStart();

                var delta = (to - from) / duration;
                var alpha = from;
                this._window.mAlpha = alpha;
                if (delta > 0f)
                {
                    while (alpha < to)
                    {
                        alpha += delta * Time.deltaTime;
                        if (alpha > to)
                        {
                            alpha = to;
                        }

                        this._window.mAlpha = alpha;
                        yield return null;
                    }
                }
                else
                {
                    while (alpha > to)
                    {
                        alpha += delta * Time.deltaTime;
                        if (alpha < to)
                        {
                            alpha = to;
                        }

                        this._window.mAlpha = alpha;
                        yield return null;
                    }
                }

                this.OnEnd();
            }
        }
    }
}