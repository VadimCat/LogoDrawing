using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Client.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public virtual UniTask AnimateShow()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask AnimateClose()
        {
            return UniTask.CompletedTask;
        }
    }
}