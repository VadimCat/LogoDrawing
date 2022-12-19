using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Client.Screens
{
    public class BackgroundService : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image backRoot;
        [SerializeField] private Sprite loadingBack;

        [SerializeField] private Sprite[] levelBackgroundImages;

        private void Awake()
        {
            SceneManager.activeSceneChanged += HandleSceneChanged;
        }

        private void HandleSceneChanged(Scene arg0, Scene arg1)
        {
            //TODO: REMOVE EAT SOME SHIT
            canvas.worldCamera = FindObjectOfType<Camera>();
            var pos = transform.position;
            pos.z = -1;
            transform.position = pos;
        }

        public void SwitchBackground(Background background)
        {
            switch (background)
            {
                case Background.Loading:
                    backRoot.sprite = loadingBack;
                    break;
                case Background.Game:
                    backRoot.sprite = levelBackgroundImages[Random.Range(0, 8)];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(background), background, null);
            }
        }
    }
}