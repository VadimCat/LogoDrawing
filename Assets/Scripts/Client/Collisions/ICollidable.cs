using System;

namespace Client.Collisions
{
    public interface ICollidable<TCollisionData>
    {
        public event Action<TCollisionData> CollisionEnter;
        public event Action<TCollisionData> CollisionStay;
        public event Action<TCollisionData> CollisionExit;
    }
}