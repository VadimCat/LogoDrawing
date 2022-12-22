using Core;
using Core.CameraProvider;
using PaintIn3D;
using UnityEngine;

namespace Client.Painting
{
    public class Painter
    {
        private readonly InputService inputService;
        private readonly CameraProvider cameraProvider;
        private readonly P3dPaintDecal paintDecal;
        
        public Painter(InputService inputService, CameraProvider cameraProvider, P3dPaintDecal paintDecal)
        {
            this.inputService = inputService;
            this.cameraProvider = cameraProvider;
            this.paintDecal = paintDecal;
        }
        
        public void Enable()
        {
            inputService.PointerMove += Paint;
        }
        
        public void Disable()
        {
            inputService.PointerMove -= Paint;
        }
        
        private void Paint(Vector2 obj)
        {
            var ray = cameraProvider.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                //Comments from P3DPaintFromCode
                var priority = 0; // If you're painting multiple times per frame, or using 'live painting', then this can be used to sort the paint draw order. This should normally be set to 0.
                var pressure = 1.0f; // If you're using modifiers that use paint pressure (e.g. from a finger), then you can set it here. This should normally be set to 1.
                var seed     = 0; // If this paint uses modifiers that aren't marked as 'Unique', then this seed will be used. This should normally be set to 0.
                var rotation = Quaternion.LookRotation(-hit.normal); // Get the rotation of the paint. This should point TOWARD the surface we want to paint, so we use the inverse normal.

                paintDecal.HandleHitPoint(false, priority, pressure, seed, hit.point, rotation);
            }
        }
    }
}