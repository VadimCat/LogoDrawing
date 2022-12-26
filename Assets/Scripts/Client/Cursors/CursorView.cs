using UnityEngine;

namespace Client.Cursors
{
    public class CursorView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public ParticleSystem Particle => particle;
        public SpriteRenderer Renderer => spriteRenderer;
    }
}