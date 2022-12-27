using UnityEngine;

namespace Client.Cursors
{
    public class CursorView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform drawPoint;
        
        public ParticleSystem Particle => particle;
        public SpriteRenderer Renderer => spriteRenderer;
        public Transform DrawPoint => drawPoint;
    }
}