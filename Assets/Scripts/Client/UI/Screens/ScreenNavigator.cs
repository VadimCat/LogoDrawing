﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Client;

namespace Client.Screens
{
    public class ScreenNavigator : MonoBehaviour, IBootstrapable
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private List<BaseScreen> screens;

        private Dictionary<Type, BaseScreen> screenOrigins;

        private BaseScreen CurrentScreen;

        public void Bootstrap()
        {
            SceneManager.sceneLoaded += SetCamera;
            
            screenOrigins = new Dictionary<Type, BaseScreen>();
            foreach (var screen in screens)
            {
                screenOrigins[screen.GetType()] = screen;
            }
        }

        private void SetCamera(Scene arg0, LoadSceneMode arg1)
        {
            canvas.worldCamera = Camera.main;
        }

        public async Task<TScreen> PushScreen<TScreen>() where TScreen : BaseScreen
        {
            if (CurrentScreen != null)
            {
                await CloseCurrent();
            }

            CurrentScreen = Instantiate(screenOrigins[typeof(TScreen)], transform);
            await CurrentScreen.AnimateShow();
            return (TScreen)CurrentScreen;
        }

        public async UniTask CloseScreen<TScreen>() where TScreen : BaseScreen
        {
            if (CurrentScreen is TScreen)
            {
                await CurrentScreen.AnimateClose();
                Destroy(CurrentScreen.gameObject);
                CurrentScreen = null;
            }
        }

        private async UniTask CloseCurrent()
        {
            await CurrentScreen.AnimateClose();
            Destroy(CurrentScreen.gameObject);
            CurrentScreen = null;
        }
    }

    public enum Background
    {
        Loading,
        Game
    }
}