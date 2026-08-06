using System.Collections.Generic;
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
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCurrentScene(string newScene)
    {
        gameData.SetCurrentScene(newScene);
    }

    public void SetKeyMappingDataList(List<KeyData> keyMappingDataList)
    {
        gameData.SetKeyMappingDataList(keyMappingDataList);
    }
}

/// <summary>
/// 저장할 데이터 정보
/// </summary>
[System.Serializable]
public struct GameData
{
    public string currentScene;
    public List<KeyData> keyMappingDataList;
    public void SetCurrentScene(string sceneName)
    {
        currentScene = sceneName;
    }

    public void SetKeyMappingDataList(List<KeyData> keyMappingDataList)
    {
        this.keyMappingDataList = keyMappingDataList;
    }
}
