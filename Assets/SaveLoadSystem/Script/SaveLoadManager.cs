using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 저장 데이터 클래스

[System.Serializable]
public class PlayerData
{
    /*
     * 스테이지 정보, 인벤토리, npc 호감도, 사운드 데이터
     */
    public List<KeyData> keyDatas;
    public int[] inventoryDatas;
    public AffinityDatas[] npcAffinityDatas;
    public VolumeDatas volumeDatas;

    public void SetKeyDatas(List<KeyData> keyDatas)
    {
        this.keyDatas = keyDatas;
    }
    public void SetInvenoryDatas(int[] itemsIdDatas)
    {
        this.inventoryDatas = itemsIdDatas;
    }
    public void SetnpcAffinityDatas(AffinityDatas[] npcAffinityDatas)
    {
        this.npcAffinityDatas = npcAffinityDatas;
    }
    public void SetVolumeDatas(VolumeDatas volumeDatas)
    {
        this.volumeDatas = volumeDatas;
    }
}
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public PlayerData nowPlayer; // 저장 데이터

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
        nowPlayer = new PlayerData();

        // 키 데이터 수집
        nowPlayer.SetKeyDatas(KeyManager.Instance.ToKeyDataList());

        // 인벤토리 데이터 수집
        nowPlayer.SetInvenoryDatas(Inventory.Instance.GetInventoryData());

        // 사운드 볼륨 데이터 수집
        nowPlayer.SetVolumeDatas(SoundManager.Instance.GetVolumeDatas());

        // npc 호감도 데이터 수집
        nowPlayer.SetnpcAffinityDatas(GameState.Instance.GetAffinityDatas().ToArray());

        // Json으로 저장
        string data = JsonUtility.ToJson(nowPlayer);
        Debug.Log($"저장 데이터: {data}");
        File.WriteAllText(GetFilePath(), data);
    }

    // 데이터 로드
    public void LoadData()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            Debug.Log($"로드 데이터: {data}");
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            Debug.LogWarning($"Save file not found: {path}");
        }
    }
}
