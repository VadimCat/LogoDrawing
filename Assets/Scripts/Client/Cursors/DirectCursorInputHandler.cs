using Client.Painting;
using Core.Camera;
using UnityEngine;

namespace Client.Cursors
{
    public class DirectCursorInputHandler : ICursorInputHandler
    {
        private readonly CameraProvider cameraProvider;
        private readonly Rigidbody2D rigidbody;

        public DirectCursorInputHandler(CameraProvider cameraProvider, Rigidbody2D rigidbody)
        {
            this.cameraProvider = cameraProvider;
            this.rigidbody = rigidbody;
        }

        public void HandlePointerDown(Vector2 inputPos)
        { }

        public void HandlePointerMove(Vector2 inputPos)
        {
            Vector3 newPos = cameraProvider.MainCamera.ScreenToWorldPoint(inputPos);
            newPos.z = rigidbody.transform.position.z;
            rigidbody.MovePosition(newPos);
        }

        public void HandlePointerUp(Vector2 inputPus)
        { }
    }
}