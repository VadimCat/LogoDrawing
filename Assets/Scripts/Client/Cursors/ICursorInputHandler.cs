using UnityEngine;

namespace Client.Cursors
{
    public interface ICursorInputHandler
    {
        public void HandleFromInput(Vector3 worldPos);
    }
}