using Client.Audio;
using Client.Collisions;
using Client.Painting;
using Core;
using UnityEngine;

namespace Client.Cursors
{
    public class ColoringCursorPresenter : CursorPresenterBase
    {
        public ColoringCursorPresenter(InputService inputService, Trigger2DEventReceiver triggerEventReceiver, CursorViewData cursorViewData, Painter painter, ICursorInputHandler cursorInputHandler, AudioService audioService, Transform cursorRoot) : base(inputService, triggerEventReceiver, cursorViewData, painter, cursorInputHandler, audioService, cursorRoot)
        {
        }

        protected override void OnPointerUp()
        {
            
            TriggerEventReceiver.EnableSimulation(false);
            SfxPlaybackSource.Stop();
            View.Particle.Stop();
        }

        protected override void OnPointerDown()
        {
            if(!isEnabled)
                return;
            
            TriggerEventReceiver.EnableSimulation(true);
            SfxPlaybackSource.PlaybackAsync(true);
            View.Particle.Play();
        }

        protected override void OnCollisionStay(Collider2D collider2D)
        {
        }

        protected override void OnCollisionExit(Collider2D obj)
        {
        }

        protected override void OnCollisionEnter(Collider2D obj)
        {
        }

        protected override void AbstractEnable()
        {
            View.gameObject.SetActive(true);
        }

        protected override void AbstractDisable()
        {
            View.gameObject.SetActive(false);
        }
    }
}