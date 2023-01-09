using Client.Painting;
using Ji2Core.Core;
using Ji2Core.Core.Audio;
using Ji2Core.Core.Collisions;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace Client.Cursors
{
    public class CursorService : MonoBehaviour, IBootstrapable
    {
        [SerializeField] private Trigger2DEventReceiver trigger2DEventReceiver;
        [SerializeField] private CursorViewData cleaningCursorViewData;
        [SerializeField] private CursorViewData coloringCursorViewData;

        public Rigidbody2D cursorRigidbody => trigger2DEventReceiver.Rigidbody;
        
        private CursorPresenterBase cleaningCursor;
        private CursorPresenterBase coloringCursor;

        private CursorPresenterBase currentCursor;
        
        private InputService inputService;
        private AudioService audioService;
        private Painter painter;
        private CursorInputHandlerFactory cursorInputHandlerFactory;

        private Vector3 initialPos;


        public void SetDependencies(InputService inputService, AudioService audioService, Painter painter,
            CursorInputHandlerFactory cursorInputHandlerFactory)
        {
            this.audioService = audioService;
            this.inputService = inputService;
            this.painter = painter;
            this.cursorInputHandlerFactory = cursorInputHandlerFactory;
        }
        
        public void Bootstrap()
        {
            initialPos = trigger2DEventReceiver.transform.position;
            
            cleaningCursor = new CleaningBrushCursorPresenter(
                painter, 
                inputService,
                trigger2DEventReceiver,
                cleaningCursorViewData,
                cursorInputHandlerFactory.Create<JoystickInputHandler>(),
                audioService,
                trigger2DEventReceiver.transform);
            cleaningCursor.Disable();

            coloringCursor = new ColoringCursorPresenter(
                painter, 
                inputService,
                trigger2DEventReceiver,
                coloringCursorViewData,
                cursorInputHandlerFactory.Create<DirectCursorInputHandler>(),
                audioService,
                trigger2DEventReceiver.transform);
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

            trigger2DEventReceiver.transform.position = initialPos;
        }

        private void OnDestroy()
        {
            DisableCurrent();
        }
    }
}