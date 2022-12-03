using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class TweenedAnimations
    {
        public static Tween DoPulseScale(this Transform transform, float maxScale, float duraion, GameObject link,
            Ease ease = Ease.Linear, int loops = -1)
        {
            return transform.DOScale(maxScale, duraion)
                .SetLoops(-1)
                .SetEase(Ease.Linear)
                .SetLink(link);
        }
    }
}