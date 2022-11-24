using Client;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InEditorBootstraper : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (!FindObjectOfType<Bootstrap>())
        {
            SceneManager.LoadScene(0);
        }
}
}
