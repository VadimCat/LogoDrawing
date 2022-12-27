using System;
using UnityEngine;

namespace Utils
{
    public class ScreenCollider2DAdjuster : MonoBehaviour
    {
        [SerializeField] private SnapAxis axis; 
        [SerializeField] private BoxCollider2D collider;
        
        private void Awake()
        {
            switch (axis)
            {
                case SnapAxis.X:
                    var sizeX = collider.size;
                    sizeX.x = Screen.width;
                    collider.size = sizeX;
                    break;
                case SnapAxis.Y:
                    var sizeY = collider.size;
                    sizeY.y = Screen.height;
                    collider.size = sizeY;
                    break;
                case SnapAxis.None:
                case SnapAxis.Z:
                case SnapAxis.All:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
