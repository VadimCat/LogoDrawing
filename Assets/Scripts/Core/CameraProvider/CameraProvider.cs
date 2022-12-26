using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Camera
{
    public class CameraProvider
    {
        private UnityEngine.Camera mainCamera;

        public UnityEngine.Camera MainCamera => mainCamera;

        public CameraProvider()
        {
            SceneManager.sceneLoaded += ChangeCamera;
        }

        private void ChangeCamera(Scene arg0, LoadSceneMode arg1)
        {
            mainCamera = UnityEngine.Camera.main;
        }
    }
}