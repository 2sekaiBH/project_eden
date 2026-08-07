using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public GameData gameData;
    public GameData GameData => gameData;

    public StageType lastStage;
    public StageType LastStage => lastStage;
    /*
    [Header("Settings")]
    [SerializeField] private List<string> stageList = new List<string>() { "floor_67", "floor_213", "floor_399" };
    public List<string> StageList => stageList;
    */

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
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
        foreach(StageType stage in System.Enum.GetValues(typeof(StageType)))
        {
            if(stage.ToString().Equals(newScene))
            {
                lastStage = stage;
                break;
            }
        }
 
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

public enum StageType
{
    floor_67,
    floor_213,
    floor_399
}
