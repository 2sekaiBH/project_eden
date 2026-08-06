using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private static KeyManager instance;
    public static KeyManager Instance // lazy singleton
    {
        get
        {
            if(instance == null) 
            {
                instance = FindAnyObjectByType<KeyManager>();
            }
            return instance;
        }
    }

    private Dictionary<KeyBindingName, KeyCode> keyMappingDict = new Dictionary<KeyBindingName, KeyCode>();

    /// <summary>
    /// 키 맵핑 정보가 변경되었음을 알리는 이벤트
    /// </summary>
    public event Action OnKeyChanged;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        ResetKeyMapping();
    }

    void Start()
    {
        GameManager.Instance.SetKeyMappingDataList(ToKeyDataList()); // 초기화된 키맵핑 정보 게임매니저에 저장
    }


    public KeyCode GetKeyCode(KeyBindingName name)
    {
        if (keyMappingDict.TryGetValue(name, out KeyCode keyCode))
        {
            return keyCode;
        }

        Debug.LogWarning($"'{name}'에 해당하는 키 매핑이 없습니다.");
        return KeyCode.None;
    }
    void ResetKeyMapping()
    {
        keyMappingDict.Clear();
        keyMappingDict[KeyBindingName.PlayerRight] = KeyCode.D;
        keyMappingDict[KeyBindingName.PlayerLeft] = KeyCode.A;
        keyMappingDict[KeyBindingName.PlayerSlide] = KeyCode.S;
        keyMappingDict[KeyBindingName.PlayerJump] = KeyCode.W;
        keyMappingDict[KeyBindingName.Interaction] = KeyCode.F;
        keyMappingDict[KeyBindingName.Settings] = KeyCode.Escape;
        keyMappingDict[KeyBindingName.Dialogue] = KeyCode.Space;
        keyMappingDict[KeyBindingName.Inventory] = KeyCode.I;
    }

    public bool SetKeyMapping(KeyBindingName name, KeyCode newKeyCode)
    {
        // 1. 이미 다른 액션에 같은 키가 바인딩되어 있으면 중복이므로 막기
        bool isDuplicate = keyMappingDict
            .Any(pair => pair.Value == newKeyCode && pair.Key != name);

        if (isDuplicate)
        {
            Debug.LogWarning($"'{newKeyCode}' 키는 이미 다른 기능에 바인딩되어 있습니다.");
            return false;
        }

        // 2. Dictionary라서 있든 없든 그냥 덮어쓰기 하나로 끝
        keyMappingDict[name] = newKeyCode;
        OnKeyChanged?.Invoke();

        GameManager.Instance.SetKeyMappingDataList(ToKeyDataList()); // 게임매니저에 저장
        return true;
    }

    // 저장할 때만 List로 변환
    public List<KeyData> ToKeyDataList()
    {
        return keyMappingDict
            .Select(pair => new KeyData { KeyName = pair.Key, keyCode = pair.Value })
            .ToList();
    }

    // 불러올 때 List -> Dictionary로 복원
    public void LoadFromKeyDataList(List<KeyData> list)
    {
        keyMappingDict = list.ToDictionary(data => data.KeyName, data => data.keyCode);
    }
}

public enum KeyBindingName
{
    PlayerRight,
    PlayerLeft,
    PlayerJump,
    PlayerSlide,
    Interaction,
    Settings,
    Dialogue,
    Inventory
}

[System.Serializable]
public class KeyData
{
    public KeyBindingName KeyName;
    public KeyCode keyCode;
}
