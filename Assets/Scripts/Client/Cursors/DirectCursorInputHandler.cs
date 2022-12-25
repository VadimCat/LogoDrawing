using Client.Painting;
using Core.Camera;
using UnityEngine;

namespace Client.Cursors
{
    public class DirectCursorInputHandler : ICursorInputHandler
    {
        private readonly CameraProvider cameraProvider;
        private readonly Painter painter;
        private readonly Rigidbody2D rigidbody;

        public DirectCursorInputHandler(CameraProvider cameraProvider, Painter painter, Rigidbody2D rigidbody)
        {
            this.cameraProvider = cameraProvider;
            this.painter = painter;
            this.rigidbody = rigidbody;
        }

        public void HandlePointerDown(Vector2 inputPos)
        { }

        public void HandlePointerMove(Vector2 inputPos)
        {
            Vector3 newPos = cameraProvider.MainCamera.ScreenToWorldPoint(inputPos);
            newPos.z = rigidbody.transform.position.z;
            rigidbody.MovePosition(newPos);
            painter.Paint(rigidbody.transform.position);
        }

        public void HandlePointerUp(Vector2 inputPus)
        { }
    }
}