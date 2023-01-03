using Ji2Core.Core;
using UnityEngine;

namespace Client.Cursors
{
    public class DirectCursorInputHandler : ICursorInputHandler
    {
        private readonly CameraProvider cameraProvider;
        private readonly Rigidbody2D rigidbody;
        private readonly DirectCursorInputHandlerConfig config;

        public DirectCursorInputHandler(CameraProvider cameraProvider, Rigidbody2D rigidbody,
            DirectCursorInputHandlerConfig directCursorInputHandlerConfig)
        {
            this.cameraProvider = cameraProvider;
            this.rigidbody = rigidbody;
            this.config = directCursorInputHandlerConfig;
        }

        public void HandlePointerDown(Vector2 inputPos)
        {
        }

        public void HandlePointerMove(Vector2 inputPos)
        {
            Vector3 newPos = cameraProvider.MainCamera.ScreenToWorldPoint(inputPos) + (Vector3)config.PointerOffset;
            newPos.z = rigidbody.transform.position.z;
            rigidbody.MovePosition(newPos);
        }

        public void HandlePointerUp(Vector2 inputPus)
        {
        }
    }
}