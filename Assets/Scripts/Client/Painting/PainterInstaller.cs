using Core;
using Core.Camera;
using Core.Installers;
using PaintIn3D;
using SceneView;
using UnityEngine;

namespace Client.Painting
{
    public class PainterInstaller : MonoInstaller<Painter>
    {
        [SerializeField] private P3dPaintDecal paintDecal;
        [SerializeField] private ColoringConfig coloringConfig;
        
        protected override Painter Create(Context context)
        {
            paintDecal.Radius = coloringConfig.BrushRadius;
            return new Painter(context.GetService<CameraProvider>(), paintDecal);
        }
    }
}