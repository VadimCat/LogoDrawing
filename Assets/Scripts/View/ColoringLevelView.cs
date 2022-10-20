using PaintIn3D;
using UnityEngine;

namespace SceneView
{
    public class ColoringLevelView : MonoBehaviour
    {
        [SerializeField] private P3dChangeCounter counter;

        public float ProgressRatio => 1 - counter.Ratio;
    }
}