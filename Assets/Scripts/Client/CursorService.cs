using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Pools;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class CursorService : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cursor;

        [SerializeField] private Sprite brush;
        [SerializeField] private Sprite sprayCan;

        private SfxPlaybackSource coloringPlaybackSource;
        private SfxPlaybackSource cleaningPlaybackSource;

        private AudioService audioService;
        private InputService inputService;

        private readonly Vector3 disabledPos = new Vector3(100, 100, 100);
        private bool isEnabled;

        public Vector2 PointerScreenPosition { get; private set; }

        public void SetDependencies(InputService inputService, AudioService audioService)
        {
            this.inputService = inputService;
            this.audioService = audioService;
        }

        public void SetBrush()
        {
            Enable();

            cursor.sprite = brush;
            cursor.DOColor(Color.white, 0);
        }

        public void SetSpray()
        {
            Enable();

            cursor.sprite = sprayCan;
            cursor.DOColor(Color.white, 0);
        }

        private void Enable()
        {
            inputService.PointerMove += OnPointerMove;
            inputService.PointerDown += OnPointerDown;
            inputService.PointerUp += OnPointerUp;
        }

        public void Disable()
        {
            inputService.PointerMove -= OnPointerMove;
            inputService.PointerDown -= OnPointerDown;
            inputService.PointerUp -= OnPointerUp;
        }

        private void OnPointerDown()
        {
            gameObject.SetActive(true);
        }

        private void OnPointerUp()
        {
            gameObject.SetActive(false);
        }

        private void OnPointerMove(Vector2 mousePoint)
        {
            float z = transform.position.z;
            PointerScreenPosition = mousePoint;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(PointerScreenPosition);

            newPos.z = z;
            transform.position = newPos;
        }

        private void OnDestroy()
        {
            Disable();
        }
    }
}