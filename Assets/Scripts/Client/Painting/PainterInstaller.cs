using Core;
using Core.CameraProvider;
using Core.Installers;
using PaintIn3D;
using UnityEngine;

namespace Client.Painting
{
    public class PainterInstaller : MonoInstaller<Painter>
    {
        [SerializeField] private P3dPaintDecal paintDecal;

        protected override Painter Create(Context context)
        {
            return new Painter(context.GetService<InputService>(), context.GetService<CameraProvider>(), paintDecal);
        }
    }
}