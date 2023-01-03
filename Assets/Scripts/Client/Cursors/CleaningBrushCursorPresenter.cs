using Client.Painting;
using Ji2Core.Core.Audio;
using Ji2Core.Core.Collisions;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace Client.Cursors
{
    public class CleaningBrushCursorPresenter : CursorPresenterBase
    {
        public CleaningBrushCursorPresenter(Painter painter, InputService inputService, Trigger2DEventReceiver triggerEventReceiver,
            CursorViewData cursorViewData, ICursorInputHandler cursorInputHandler,
            AudioService audioService, Transform cursorRoot) :
            base(painter, inputService, triggerEventReceiver, cursorViewData, cursorInputHandler, audioService, cursorRoot)
        {
        }

        protected override void OnPointerMoveAbstract(Vector2 inputPos)
        { }

        protected override void OnPointerUpAbstract(Vector2 inputPos)
        {
            TriggerEventReceiver.EnableSimulation(false);
        }

        protected override void OnPointerDownAbstract(Vector2 inputPos)
        {
            if(!isEnabled)
                return;
            
            TriggerEventReceiver.EnableSimulation(true);
        }

        protected override void OnCollisionEnter(Collider2D obj)
        {
            if(!isEnabled)
                return;
            
            View.Particle.Play();
            SfxPlaybackSource.PlaybackAsync(true);
        }

        protected override void EnableAbstract()
        {            
            View.gameObject.SetActive(true);
        }

        protected override void DisableAbstract()
        {
            View.gameObject.SetActive(false);
        }

        protected override void OnCollisionStay(Collider2D collider2D)
        {
        }

        protected override void OnCollisionExit(Collider2D obj)
        {
            View.Particle.Stop();
            SfxPlaybackSource.Stop();
        }
    }
}