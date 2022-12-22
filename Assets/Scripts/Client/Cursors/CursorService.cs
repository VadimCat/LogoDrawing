using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Collisions;
using Client.Painting;
using Core;
using UnityEngine;

namespace Client.Cursors
{
    public class CursorService : MonoBehaviour
    {
        [SerializeField] private Trigger2DEventReceiver trigger2DEventReceiver;
        [SerializeField] private CursorView cleaningCursorView;
        [SerializeField] private CursorView coloringCursorView;

        private readonly Vector3 disabledPosDelta = new(20, 20);
        
        private ICursorInputHandler inputHandler;
        private CursorView currentCursorView;
        
        private SfxPlaybackSource sfxPlaybackSource;

        private InputService inputService;

        private bool isEnabled;

        public Vector2 PointerScreenPosition { get; private set; }

        public void SetDependencies(InputService inputService, AudioService audioService)
        {
            this.inputService = inputService;
            
            // inputHandler = new DirectCursorInputHandler()
            
            cleaningCursorView.SetDependencies(audioService);
            coloringCursorView.SetDependencies(audioService);
        }
        
        //TODO: Make common method for all brushes activation 
        public void SetCleaning()
        {
            currentCursorView?.Stop();
            DisableCurrent();
            SubscribeEvents();

            currentCursorView = cleaningCursorView;
            currentCursorView.Play();
        }

        public void SetColoring()
        {
            currentCursorView?.Stop();
            DisableCurrent();
            SubscribeEvents();

            currentCursorView = coloringCursorView;
            currentCursorView.Play();
        }

        public void DisableCurrent()
        {
            trigger2DEventReceiver.CollisionEnter -= HandleCollisionEnter;
            trigger2DEventReceiver.CollisionExit -= HandleCollisionExit;
            inputService.PointerMoveScreenSpace -= PointerMoveScreenSpace;
            inputService.PointerDown -= OnPointerDown;
            inputService.PointerUp -= OnPointerUp;
            
            
            transform.Translate(disabledPosDelta);
            currentCursorView?.Stop();
            currentCursorView = null;
        }

        private void SubscribeEvents()
        {
            trigger2DEventReceiver.CollisionEnter += HandleCollisionEnter;
            trigger2DEventReceiver.CollisionExit += HandleCollisionExit;
            inputService.PointerMoveScreenSpace += PointerMoveScreenSpace;
            inputService.PointerDown += OnPointerDown;
            inputService.PointerUp += OnPointerUp;
        }

        private void HandleCollisionExit(Collider2D obj)
        {
            currentCursorView.StopSfx();
        }

        private void HandleCollisionEnter(Collider2D obj)
        {
            currentCursorView.PlaySfx();
        }

        private void OnPointerDown()
        {
            gameObject.SetActive(true);
        }

        private void OnPointerUp()
        {
            gameObject.SetActive(false);
        }

        private void PointerMoveScreenSpace(Vector3 mousePoint)
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

    public abstract class CursorPresenterBase
    {
        private readonly Trigger2DEventReceiver eventReceiver;
        private readonly AudioService audioService;
        private readonly SfxPlaybackSource sfxPlaybackSource;
        private readonly CursorView cursorView;
        
        public CursorPresenterBase(CursorViewData cursorViewData, Painter painter, Trigger2DEventReceiver eventReceiver,
            ICursorInputHandler cursorInputHandler, AudioService audioService)
        {
            this.eventReceiver = eventReceiver;
            this.audioService = audioService;

            sfxPlaybackSource = audioService.GetPlaybackSource(cursorViewData.ClipName);
            cursorView = Object.Instantiate(cursorView);
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}