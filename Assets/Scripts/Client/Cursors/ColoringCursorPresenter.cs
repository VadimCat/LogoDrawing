using Client.Audio;
using Client.Collisions;
using Client.Painting;
using Core.UserInput;
using UnityEngine;

namespace Client.Cursors
{
    public class ColoringCursorPresenter : CursorPresenterBase
    {
        public ColoringCursorPresenter(Painter painter, InputService inputService, Trigger2DEventReceiver triggerEventReceiver,
            CursorViewData cursorViewData, ICursorInputHandler cursorInputHandler, AudioService audioService,
            Transform cursorRoot) : base(painter, inputService, triggerEventReceiver, cursorViewData,
            cursorInputHandler, audioService, cursorRoot)
        {
        }

        protected override void OnPointerUpAbstract(Vector2 inputPos)
        {
            TriggerEventReceiver.EnableSimulation(false);
            SfxPlaybackSource.Stop();
            View.Particle.Stop();
        }

        protected override void OnPointerDownAbstract(Vector2 inputPos)
        {
            if (!isEnabled)
                return;

            TriggerEventReceiver.EnableSimulation(true);
            SfxPlaybackSource.PlaybackAsync(true);
            View.Particle.Play();
        }

        protected override void OnPointerMoveAbstract(Vector2 inputPos)
        {
            if (isEnabled && !SfxPlaybackSource.IsPlaying)
            {
                SfxPlaybackSource.PlaybackAsync(true);
            }
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

        protected override void EnableAbstract()
        {
            View.gameObject.SetActive(true);
        }

        protected override void DisableAbstract()
        {
            SfxPlaybackSource.Stop();
            View.gameObject.SetActive(false);
        }
    }
}