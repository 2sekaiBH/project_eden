using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private string itemUISceneName = "Scene_Init";

    private void Start()
    {
        if (!SceneManager.GetSceneByName(itemUISceneName).isLoaded)
        {
            SceneManager.LoadScene(itemUISceneName, LoadSceneMode.Additive);
        }
    }
}