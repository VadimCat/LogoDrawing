using UnityEngine;

namespace Client.Cursors
{
    [CreateAssetMenu(menuName = "DirectCursorInputHandlerConfig", fileName = "DirectCursorInputHandlerConfig")]
    public class DirectCursorInputHandlerConfig : ScriptableObject
    {
        [SerializeField] private Vector2 pointerOffset;

        public Vector2 PointerOffset => pointerOffset;
    }
}