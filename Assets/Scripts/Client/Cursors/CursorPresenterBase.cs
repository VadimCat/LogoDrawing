using Client.Audio;
using Client.Audio.SfxPlayers;
using Client.Collisions;
using Client.Painting;
using Core;
using UnityEngine;

namespace Client.Cursors
{
    public abstract class CursorPresenterBase
    {
        protected readonly Vector2 DisabledDelta = new Vector2(10, 10);
        protected readonly Painter Painter;
        protected readonly Trigger2DEventReceiver TriggerEventReceiver;
        protected readonly ICursorInputHandler CursorInputHandler;
        protected readonly InputService InputService;
        protected readonly AudioService AudioService;
        protected readonly SfxPlaybackSource SfxPlaybackSource;
        protected readonly CursorView View;

        protected bool isEnabled;
        
        public CursorPresenterBase(InputService inputService, Trigger2DEventReceiver triggerEventReceiver,
            CursorViewData cursorViewData, Painter painter, ICursorInputHandler cursorInputHandler,
            AudioService audioService, Transform cursorRoot)
        {
            this.Painter = painter;
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

        private void OnPointerMove(Vector3 inputPos)
        {
            if(!isEnabled)
                return;
            
            CursorInputHandler.HandleFromInput(inputPos);
            Painter.Paint(inputPos);
        }

        //TODO: Use aggregation with classes like CursorParticleBehaviour,
        //TODO: which will be aggregated inside class and will solve their problems on their own
        protected abstract void OnPointerUp();
        protected abstract void OnPointerDown();
        protected abstract void OnCollisionStay(Collider2D collider2D);

        protected abstract void OnCollisionExit(Collider2D obj);
        protected abstract void OnCollisionEnter(Collider2D obj);

        protected abstract void AbstractEnable();

        protected abstract void AbstractDisable();

        public void Enable()
        {
            isEnabled = true;
            AbstractEnable();
        }

        public void Disable()
        {
            isEnabled = false;

            AbstractDisable();
        }
    }
}