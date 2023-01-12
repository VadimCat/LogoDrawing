using UnityEngine;

namespace Client.View
{
    [CreateAssetMenu]
    public class ColoringConfig : ScriptableObject
    {
        [SerializeField] private float brushRadius = .3f;

        public float BrushRadius => brushRadius;
    }
}