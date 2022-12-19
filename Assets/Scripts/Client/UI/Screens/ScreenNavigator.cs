using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils.Client;

namespace Client.Screens
{
    public class ScreenNavigator : MonoBehaviour, IBootstrapable
    {
        [SerializeField] private List<BaseScreen> screens;

        private Dictionary<Type, BaseScreen> screenOrigins;

        private BaseScreen CurrentScreen;

        public void Bootstrap()
        {
            screenOrigins = new Dictionary<Type, BaseScreen>();
            foreach (var screen in screens)
            {
                screenOrigins[screen.GetType()] = screen;
            }
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