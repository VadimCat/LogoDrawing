using PaintIn3D;
using UnityEngine;

namespace SceneView
{
    public class ColoringLevelView : MonoBehaviour
    {
        [SerializeField] private P3dChangeCounter counter;
        [SerializeField] private P3dPaintDecal decal;
        
        [SerializeField] private ColoringConfig coloringConfig;
        
        private void Awake()
        {
            decal.Radius = coloringConfig.BrushRadius;
        }

        public float ProgressRatio => counter.Ratio;
    }
}