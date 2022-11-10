using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client.Screens
{
    public class BackGroundService : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image backRoot;
        [SerializeField] private Sprite loadingBack;
        [SerializeField] private Sprite gameBack;

        private void Awake()
        {
            SceneManager.activeSceneChanged += HandleSceneChanged;
        }

        private void HandleSceneChanged(Scene arg0, Scene arg1)
        {
            //EAT SOME SHIT
            canvas.worldCamera = FindObjectOfType<Camera>();
        }

        public void SwitchBackground(Background background)
        {
            switch (background)
            {
                case Background.Loading:
                    backRoot.sprite = loadingBack;
                    break;
                case Background.Game:
                    backRoot.sprite = gameBack;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(background), background, null);
            }
        }
        
    }
}