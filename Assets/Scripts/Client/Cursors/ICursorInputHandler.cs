using UnityEngine;

namespace Client.Cursors
{
    public interface ICursorInputHandler
    { 
        public void HandlePointerDown(Vector2 inputPos);
        public void HandlePointerMove(Vector2 inputPos);
        public void HandlePointerUp(Vector2 inputPus);
    }
}