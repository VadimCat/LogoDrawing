using System;
using UnityEngine;

namespace Core
{
    public class InputService : IUpdatable, IDisposable
    {
        private readonly UpdateService updateService;
        public event Action<Vector2> PointerMove;
        public event Action PointerDown;
        public event Action PointerUp;

        private bool isEnabled;

        public InputService(UpdateService updateService)
        {
            this.updateService = updateService;
            
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
                PointerMove?.Invoke(Input.mousePosition);
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
            PointerMove = null;
            PointerDown = null;
            PointerUp = null;
        }
    }
}