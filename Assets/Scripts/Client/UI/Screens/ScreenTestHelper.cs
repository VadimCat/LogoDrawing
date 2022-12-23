using System;
using UnityEngine;

namespace Client.Screens
{
    public class ScreenTestHelper : MonoBehaviour
    {
        [SerializeField] private BaseScreen BaseScreen;
        [SerializeField] private ScreenNavigator screenNavigator;

        private void Start()
        {
            screenNavigator.Bootstrap();
            var type = BaseScreen.GetType();

            screenNavigator.PushScreen(type);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var type = BaseScreen.GetType();
                Debug.LogError(type);
                
                screenNavigator.PushScreen(type);
            }
        }
    }
}