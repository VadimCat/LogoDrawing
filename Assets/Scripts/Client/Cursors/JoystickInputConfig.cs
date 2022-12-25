using UnityEngine;

namespace Client.Cursors
{
    [CreateAssetMenu(menuName = "JoystickInputConfig", fileName = "JoystickInputConfig", order = 0)]
    public class JoystickInputConfig : ScriptableObject
    {
        [SerializeField] private float speed;

        public float Speed => speed;
    }
}