using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 저장 데이터 클래스
public class PlayerData
{
    /*
    //위치, 스토리 진행도, 체력, 멘탈, 소지 아이템 
    public int HP;
    public float Mental;
    public InventorySaveData InventorySaveData;
    public Vector3 Location;
    public string[] Flags;
    public List<QuestSaveData> Quests;
    public string SaveTime;
    public string currentScene;
    public PlayerData(int HP, float Mental, InventorySaveData InventorySaveData, Vector3 Location, string[] Flags, List<QuestSaveData> Quests, string SaveTime, string currentScene)
    {
        this.HP = HP;
        this.Mental = Mental;
        this.InventorySaveData = InventorySaveData;
        this.Location = Location;
        this.Flags = Flags;
        this.Quests = Quests;
        this.SaveTime = SaveTime;
        this.currentScene = currentScene;
    }*/
}
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // 저장 경로 반환
    public string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, $"save.json");
    }

    // 데이터 수집해 저장
    public void SaveData()
    {
        /*
        InventorySaveData = new InventorySaveData(inventory);

        string[] flags = FlagManager.Instance.flagsData;
        List<QuestSaveData> quests = QuestManager.Instance.SaveActiveQuest();

        string SaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int Hp = playerSaveData.Hp;
        float Mental = playerSaveData.Mental;
        Vector3 Location = playerSaveData.PlayerLocation;

        nowPlayer = new PlayerData(Hp, Mental, InventorySaveData, Location, flags, quests, SaveTime, currentScene);
        string data = JsonUtility.ToJson(nowPlayer);
        Debug.Log(data);
        File.WriteAllText(GetFilePath((nowSlot)), data);*/
    }

    // 데이터 로드
    public void LoadData()
    {
        /*
        string path = GetFilePath();
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            Debug.Log(data);
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            Debug.LogWarning($"Save file not found: {path}");
        }
        */
    }
}
