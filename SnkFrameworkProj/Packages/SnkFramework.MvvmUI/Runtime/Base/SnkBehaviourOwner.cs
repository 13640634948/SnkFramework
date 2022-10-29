using UnityEngine;
using UnityEngine.EventSystems;

namespace SnkFramework.Mvvm.Runtime
{
    namespace Base
    {
        [RequireComponent(typeof(RectTransform), typeof(Canvas), typeof(CanvasGroup))]
        public abstract class SnkBehaviourOwner : UIBehaviour
        {
        }
    }
}