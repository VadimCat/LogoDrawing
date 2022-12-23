using Client.Audio;
using Client.Collisions;
using Client.Painting;
using Core;
using UnityEngine;

namespace Client.Cursors
{
    public class CleaningBrushCursorPresenter : CursorPresenterBase
    {
        public CleaningBrushCursorPresenter(InputService inputService, Trigger2DEventReceiver triggerEventReceiver,
            CursorViewData cursorViewData, Painter painter, ICursorInputHandler cursorInputHandler,
            AudioService audioService, Transform cursorRoot) :
            base(inputService, triggerEventReceiver, cursorViewData, painter, cursorInputHandler, audioService, cursorRoot)
        {
        }

        protected override void OnPointerUp()
        {
            TriggerEventReceiver.EnableSimulation(false);
        }

        protected override void OnPointerDown()
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

        protected override void AbstractEnable()
        {            
            View.gameObject.SetActive(true);
        }

        protected override void AbstractDisable()
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