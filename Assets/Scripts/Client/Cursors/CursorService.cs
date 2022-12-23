using Client.Audio;
using Client.Collisions;
using Client.Painting;
using Core;
using Core.CameraProvider;
using UnityEngine;
using Utils.Client;

namespace Client.Cursors
{
    public class CursorService : MonoBehaviour, IBootstrapable
    {
        [SerializeField] private Trigger2DEventReceiver trigger2DEventReceiver;
        [SerializeField] private CursorViewData cleaningCursorViewData;
        [SerializeField] private CursorViewData coloringCursorViewData;

        private readonly Vector3 disabledPosDelta = new(20, 20);
        private CursorPresenterBase cleaningCursor;
        private CursorPresenterBase coloringCursor;

        private CursorPresenterBase currentCursor;
        
        private InputService inputService;
        private AudioService audioService;
        private Painter painter;
        private CameraProvider cameraProvider;

        public Vector2 PointerScreenPosition { get; private set; }

        public void SetDependencies(InputService inputService, AudioService audioService, Painter painter,
            CameraProvider cameraProvider)
        {
            this.audioService = audioService;
            this.inputService = inputService;
            this.painter = painter;
            this.cameraProvider = cameraProvider;
        }
        

        public void Bootstrap()
        {
            cleaningCursor = new CleaningBrushCursorPresenter(
                inputService,
                trigger2DEventReceiver,
                cleaningCursorViewData,
                painter,
                new DirectCursorInputHandler(cameraProvider, transform),
                audioService,
                transform);
            cleaningCursor.Disable();

            coloringCursor = new ColoringCursorPresenter(
                inputService,
                trigger2DEventReceiver,
                coloringCursorViewData,
                painter,
                new DirectCursorInputHandler(cameraProvider, transform),
                audioService,
                transform);
            coloringCursor.Disable();
        }

        //TODO: Make common method for all brushes activation 
        public void SetCleaning()
        {
            DisableCurrent();
            cleaningCursor.Enable();
            currentCursor = cleaningCursor;
        }

        public void SetColoring()
        {
            DisableCurrent();
            coloringCursor.Enable();
            currentCursor = coloringCursor;
        }

        public void DisableCurrent()
        {
            currentCursor?.Disable();
            currentCursor = null;
        }

        private void OnDestroy()
        {
            DisableCurrent();
        }
    }
}