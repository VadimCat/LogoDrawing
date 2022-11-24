using System;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class CursorService : MonoBehaviour, IUpdatable
    {
        [SerializeField] private SpriteRenderer cursor;
        
        [SerializeField] private Sprite brush;
        [SerializeField] private Sprite sprayCan;

        private UpdateService updateService;

        private bool isEnabled;

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
            cursor.DOColor(new Color(), 0);
        }

        public void OnUpdate()
        {
            float z = transform.position.z;

            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            newPos.z = z;
            transform.position = newPos;
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