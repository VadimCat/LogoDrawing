using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Collisions;
using UnityEngine;

namespace Client
{
    //Nihuya ne service
    public class CursorService : MonoBehaviour
    {
        [SerializeField] private Trigger2DEventReceiver trigger2DEventReceiver;
        [SerializeField] private Cursor cleaningCursor;
        [SerializeField] private Cursor coloringCursor;

        private Cursor currentCursor;
        
        private SfxPlaybackSource sfxPlaybackSource;

        private InputService inputService;

        private bool isEnabled;

        public Vector2 PointerScreenPosition { get; private set; }

        public void SetDependencies(InputService inputService, AudioService audioService)
        {
            this.inputService = inputService;
            
            cleaningCursor.SetDependencies(audioService);
            coloringCursor.SetDependencies(audioService);
        }
        
        //TODO: Make common method for all brushes activation 
        public void SetCleaning()
        {
            currentCursor?.Stop();
            DisableCurrent();
            SubscribeEvents();

            currentCursor = cleaningCursor;
            currentCursor.Play();
        }

        public void SetColoring()
        {
            currentCursor?.Stop();
            DisableCurrent();
            SubscribeEvents();

            currentCursor = coloringCursor;
            currentCursor.Play();
        }

        private void SubscribeEvents()
        {
            trigger2DEventReceiver.CollisionEnter += HandleCollisionEnter;
            trigger2DEventReceiver.CollisionExit += HandleCollisionExit;
            inputService.PointerMove += OnPointerMove;
            inputService.PointerDown += OnPointerDown;
            inputService.PointerUp += OnPointerUp;
        }

        private void HandleCollisionExit(Collider2D obj)
        {
            currentCursor.Stop();
        }

        private void HandleCollisionEnter(Collider2D obj)
        {
            currentCursor.Play();
        }

        public void DisableCurrent()
        {
            trigger2DEventReceiver.CollisionEnter -= HandleCollisionEnter;
            trigger2DEventReceiver.CollisionExit -= HandleCollisionExit;
            inputService.PointerMove -= OnPointerMove;
            inputService.PointerDown -= OnPointerDown;
            inputService.PointerUp -= OnPointerUp;
            
            currentCursor?.Stop();
            currentCursor = null;
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
            DisableCurrent();
        }
    }
}