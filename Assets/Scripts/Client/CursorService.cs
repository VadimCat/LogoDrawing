using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Collisions;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    //Nihuya ne service
    public class CursorService : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cursor;
        [SerializeField] private Trigger2DEventReceiver trigger2DEventReceiver;
        
        [SerializeField] private Sprite brush;
        [SerializeField] private Sprite sprayCan;

        private const AudioClipName cleaningSfx = AudioClipName.CleaningFX;
        private const AudioClipName colopringSfx = AudioClipName.ColoringFX;
        
        private SfxPlaybackSource sfxPlaybackSource;

        private AudioService audioService;
        private InputService inputService;

        private bool isEnabled;

        public Vector2 PointerScreenPosition { get; private set; }

        public void SetDependencies(InputService inputService, AudioService audioService)
        {
            this.inputService = inputService;
            this.audioService = audioService;
        }
        
        //TODO: Divide different cursors by different classes 
        public void SetCleaning()
        {
            Enable();
            ReleaseCurrentSfx();

            sfxPlaybackSource = audioService.GetPlaybackSource(cleaningSfx);
            
            cursor.sprite = brush;
            cursor.DOColor(Color.white, 0);
        }

        private void ReleaseCurrentSfx()
        {
            if (sfxPlaybackSource != null)
            {
                audioService.ReleasePlaybackSource(sfxPlaybackSource);
                sfxPlaybackSource = null;
            }
        }

        public void SetColoring()
        {
            Enable();
            ReleaseCurrentSfx();

            sfxPlaybackSource = audioService.GetPlaybackSource(colopringSfx);

            cursor.sprite = sprayCan;
            cursor.DOColor(Color.white, 0);
        }

        private void Enable()
        {
            trigger2DEventReceiver.CollisionEnter += PlayFx;
            trigger2DEventReceiver.CollisionExit += PauseFx;
            inputService.PointerMove += OnPointerMove;
            inputService.PointerDown += OnPointerDown;
            inputService.PointerUp += OnPointerUp;
        }
        
        public void Disable()
        {            
            ReleaseCurrentSfx();

            inputService.PointerMove -= OnPointerMove;
            inputService.PointerDown -= OnPointerDown;
            inputService.PointerUp -= OnPointerUp;
        }

        private void PauseFx(Collider2D obj)
        {
            sfxPlaybackSource?.Pause();
        }

        private void PlayFx(Collider2D obj)
        {
            sfxPlaybackSource.PlaybackAsync(true);
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