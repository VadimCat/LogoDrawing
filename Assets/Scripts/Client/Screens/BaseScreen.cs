using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Client.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public abstract UniTask AnimateShow();

        public abstract UniTask AnimateClose();
    }
}