using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Client.Screens
{
    public class BackGroundService : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image backRoot;
        [SerializeField] private Sprite loadingBack;
        [SerializeField] private Sprite gameBack;

        [SerializeField] private Sprite[] levelBackgroundImages;
        private readonly LinkedList<Sprite> backgrounds = new();
        private LinkedListNode<Sprite> currentBackground;

        private void Awake()
        {
            FillLinkedList();
            currentBackground = backgrounds.Last;
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

        private void FillLinkedList()
        {
            foreach (var t in levelBackgroundImages)
            {
                backgrounds.AddLast(t);
            }
        }

        public void SwitchBackground(Background background)
        {
            switch (background)
            {
                case Background.Loading:
                    backRoot.sprite = loadingBack;
                    break;
                case Background.Game:
                    // backRoot.sprite = levelBackgroundImages[Random.Range(0, 8)];
                    currentBackground = currentBackground.NextOrFirst();
                    backRoot.sprite = currentBackground.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(background), background, null);
            }
        }
    }
}