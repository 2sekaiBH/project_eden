using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private string itemUISceneName = "Scene_Init"; // 이제 이 씬 하나만 로드

    private void Start()
    {
        if (!SceneManager.GetSceneByName(itemUISceneName).isLoaded)
        {
            SceneManager.LoadScene(itemUISceneName, LoadSceneMode.Additive);
        }
    }
}