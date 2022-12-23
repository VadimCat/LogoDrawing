using Core.CameraProvider;
using UnityEngine;

namespace Client.Cursors
{
    public class DirectCursorInputHandler : ICursorInputHandler
    {
        private readonly CameraProvider cameraProvider;
        private readonly Transform cursorRoot;

        public DirectCursorInputHandler(CameraProvider cameraProvider, Transform cursorRoot)
        {
            this.cameraProvider = cameraProvider;
            this.cursorRoot = cursorRoot;
        }

        public void HandleFromInput(Vector3 inputPos)
        {
            Vector3 newPos = cameraProvider.MainCamera.ScreenToWorldPoint(inputPos);
            newPos.z = cursorRoot.position.z;
            cursorRoot.transform.position = newPos;
        }
    }
}