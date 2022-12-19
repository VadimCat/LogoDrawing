using System;
using UnityEngine;

namespace Client.Collisions
{
    public class Trigger2DEventReceiver : MonoBehaviour, ICollidable<Collider2D>
    {
        public event Action<Collider2D> CollisionEnter;
        public event Action<Collider2D> CollisionStay;
        public event Action<Collider2D> CollisionExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            CollisionEnter?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            CollisionExit?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            CollisionStay?.Invoke(other);
        }
    }
}