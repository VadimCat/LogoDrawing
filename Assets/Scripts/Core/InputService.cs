using System;
using UnityEngine;

namespace Core
{
    public class InputService : IUpdatable, IDisposable
    {
        private readonly UpdateService updateService;
        private readonly CameraProvider.CameraProvider cameraProvider;
        public event Action<Vector3> PointerMoveScreenSpace;
        public event Action<Vector3> PointerMoveWorldSpace;
        public event Action PointerDown;
        public event Action PointerUp;

        private bool isEnabled;

        public InputService(UpdateService updateService, CameraProvider.CameraProvider cameraProvider)
        {
            this.updateService = updateService;
            this.cameraProvider = cameraProvider;

            updateService.Add(this);
        }

        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerDown?.Invoke();
                isEnabled = true;
            }
            else if (isEnabled && Input.GetMouseButton(0))
            {
                PointerMoveScreenSpace?.Invoke(Input.mousePosition);
                PointerMoveWorldSpace?.Invoke(cameraProvider.MainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (isEnabled && Input.GetMouseButtonUp(0))
            {
                PointerUp?.Invoke();
                isEnabled = false;
            }
        }

        public void Dispose()
        {
            updateService.Remove(this);
            PointerMoveScreenSpace = null;
            PointerDown = null;
            PointerUp = null;
        }
    }
}