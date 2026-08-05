using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public GameData gameData;
    public GameData GameData => gameData;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameData = new GameData();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBGM(EBgm.Dialogue_399);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SoundManager.Instance.PlayBGM(EBgm.Dialogue_399);
        }
    }

    public void SetCurrentScene(string newScene)
    {
        gameData.SetCurrentScene(newScene);
    }
}

[System.Serializable]
public struct GameData
{
    public string currentScene;

    public void SetCurrentScene(string sceneName)
    {
        currentScene = sceneName;
    }
}
