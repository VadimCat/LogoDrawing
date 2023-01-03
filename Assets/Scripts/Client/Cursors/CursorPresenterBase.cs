using Client.Painting;
using Ji2Core.Core.Audio;
using Ji2Core.Core.Collisions;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace Client.Cursors
{
    public abstract class CursorPresenterBase
    {
        protected readonly Vector2 DisabledDelta = new Vector2(10, 10);
        private readonly Painter painter;
        protected readonly Trigger2DEventReceiver TriggerEventReceiver;
        protected readonly ICursorInputHandler CursorInputHandler;
        protected readonly InputService InputService;
        protected readonly AudioService AudioService;
        protected readonly SfxPlaybackSource SfxPlaybackSource;
        protected readonly CursorView View;

        protected bool isEnabled;
        
        public CursorPresenterBase(Painter painter, InputService inputService, Trigger2DEventReceiver triggerEventReceiver,
            CursorViewData cursorViewData, ICursorInputHandler cursorInputHandler,
            AudioService audioService, Transform cursorRoot)
        {
            this.painter = painter;
            this.TriggerEventReceiver = triggerEventReceiver;
            this.CursorInputHandler = cursorInputHandler;
            this.InputService = inputService;
            this.AudioService = audioService;

            triggerEventReceiver.CollisionEnter += OnCollisionEnter;
            triggerEventReceiver.CollisionExit += OnCollisionExit;
            triggerEventReceiver.CollisionStay += OnCollisionStay;

            inputService.PointerDown += OnPointerDown;
            inputService.PointerUp += OnPointerUp;
            inputService.PointerMoveScreenSpace += OnPointerMove;

            SfxPlaybackSource = audioService.GetPlaybackSource(cursorViewData.ClipName);
            View = Object.Instantiate(cursorViewData.View, cursorRoot);
        }

        private void OnPointerMove(Vector2 inputPos)
        {
            if(!isEnabled)
                return;
         
            OnPointerMoveAbstract(inputPos);
            CursorInputHandler.HandlePointerMove(inputPos);
            painter.Paint(View.DrawPoint.position);
        }

        private void OnPointerUp(Vector2 inputPos)
        {
            OnPointerUpAbstract(inputPos);
            CursorInputHandler.HandlePointerUp(inputPos);
        }

        private void OnPointerDown(Vector2 inputPos)
        {
            if(!isEnabled)
                return;
            
            OnPointerDownAbstract(inputPos);
            CursorInputHandler.HandlePointerDown(inputPos);
        }

        protected abstract void OnPointerDownAbstract(Vector2 inputPos);
        protected abstract void OnPointerMoveAbstract(Vector2 inputPos);
        protected abstract void OnPointerUpAbstract(Vector2 inputPos);

        //TODO: Use aggregation with classes like CursorParticleBehaviour,
        //TODO: which will be aggregated inside class and will solve their problems on their own
        protected abstract void OnCollisionStay(Collider2D collider2D);

        protected abstract void OnCollisionExit(Collider2D obj);
        protected abstract void OnCollisionEnter(Collider2D obj);

        protected abstract void EnableAbstract();

        protected abstract void DisableAbstract();

        public void Enable()
        {
            TriggerEventReceiver.gameObject.SetActive(true);
            isEnabled = true;
            EnableAbstract();
        }

        public void Disable()
        {
            isEnabled = false;
            TriggerEventReceiver.gameObject.SetActive(false);
            // OnPointerUp();
            
            DisableAbstract();
        }
    }
}