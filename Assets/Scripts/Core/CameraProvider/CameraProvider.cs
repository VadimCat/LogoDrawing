using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.CameraProvider
{
    public class CameraProvider
    {
        private Camera mainCamera;

        public Camera MainCamera => mainCamera;

        public CameraProvider()
        {
            SceneManager.sceneLoaded += ChangeCamera;
        }

        private void ChangeCamera(Scene arg0, LoadSceneMode arg1)
        {
            mainCamera = Camera.main;
        }
    }
}