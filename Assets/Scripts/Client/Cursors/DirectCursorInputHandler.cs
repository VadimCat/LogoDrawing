using Core;
using UnityEngine;

namespace Client.Cursors
{
    public class DirectCursorInputHandler : ICursorInputHandler
    {
        private readonly InputService inputService;

        public DirectCursorInputHandler(InputService inputService)
        {
            this.inputService = inputService;

            inputService.PointerMoveWorldSpace += HandleFromInput;
        }

        private void HandleFromInput(Vector2 obj)
        {
            throw new System.NotImplementedException();
        }

        public void HandleFromInput(Vector3 worldPos)
        {
            
            // pos.z = transform.position.z;
            // transform.position = pos;
        }
    }
}