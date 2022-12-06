using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class CursorService : MonoBehaviour, IUpdatable
    {
        [SerializeField] private SpriteRenderer cursor;

        [SerializeField] private Sprite brush;
        [SerializeField] private Sprite sprayCan;

        private readonly Vector3 disabledPos = new Vector3(100, 100, 100);

        private UpdateService updateService;


        private bool isEnabled;

        public Vector2 PointerScreenPosition { get; private set; }

        public void Construct(UpdateService updateService)
        {
            this.updateService = updateService;
        }

        public void SetBrush()
        {
            Enable();

            cursor.sprite = brush;
            cursor.DOColor(Color.white, 0);
        }

        public void SetSpray()
        {
            Enable();

            cursor.sprite = sprayCan;
            cursor.DOColor(Color.white, 0);
        }

        public void Disable()
        {
            updateService.Remove(this);
            gameObject.SetActive(false);
        }

        public void OnUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                gameObject.SetActive(true);

                float z = transform.position.z;
                PointerScreenPosition = Input.mousePosition;
                Vector3 newPos = Camera.main.ScreenToWorldPoint(PointerScreenPosition);

                newPos.z = z;
                transform.position = newPos;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void Enable()
        {
            updateService.Add(this);
        }

        private void OnDestroy()
        {
            updateService.Remove(this);
        }
    }
}