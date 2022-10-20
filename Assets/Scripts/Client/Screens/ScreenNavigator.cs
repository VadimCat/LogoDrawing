using System;
using System.Collections.Generic;
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

        public TScreen PushScreen<TScreen>() where TScreen : BaseScreen
        {
            if (CurrentScreen != null)
                CurrentScreen.Close();

            CurrentScreen = Instantiate(screenOrigins[typeof(TScreen)], transform);
            CurrentScreen.Show();
            return (TScreen)CurrentScreen;
        }
    }

    public abstract class BaseScreen : MonoBehaviour
    {
        public abstract UniTask Show();

        public abstract UniTask Close();
    }
}