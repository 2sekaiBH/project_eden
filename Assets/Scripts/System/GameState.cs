using System.Collections.Generic;
using UnityEngine;

public enum FactionType
{
    None,
    Archi,
    Eve,
    Noa
}

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private string playerName = "";

    [Header("Affinity")]
    [SerializeField] private int architectAffinity;
    [SerializeField] private int cainAffinity;
    [SerializeField] private int noahAffinity;

    [Header("Hidden Ending Item IDs")]
    [Tooltip("Noa 히든 엔딩에 필요한 아이템 ID 5개를 입력하세요.")]
    [SerializeField]
    private int[] hiddenEndingItemIds =
    {
        5,6,7,8,9
    };

    [Header("Final Choice")]
    [SerializeField] private FactionType selectedFaction = FactionType.None;
    public FactionType SelectedFaction => selectedFaction;


    // 실제로 획득한 히든 엔딩 아이템 ID.
    // HashSet을 사용하므로 같은 아이템을 중복 획득해도 한 번만 기록됨.
    private readonly HashSet<int> acquiredHiddenEndingItemIds =
        new HashSet<int>();

    public string PlayerName => playerName;

    // 실제로 등록된 히든 엔딩 아이템의 획득 수
    public int AcquiredHiddenEndingItemCount =>
        acquiredHiddenEndingItemIds.Count;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInstanceIfNeeded()
    {
        if (Instance != null)
        {
            return;
        }

        GameState existing =
            FindFirstObjectByType<GameState>();

        if (existing != null)
        {
            Instance = existing;
            DontDestroyOnLoad(existing.gameObject);
            return;
        }

        GameObject gameStateObject =
            new GameObject("[GameState]");

        gameStateObject.AddComponent<GameState>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName(string value)
    {
        playerName = value?.Trim() ?? "";
    }

    public string ReplacePlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        return value.Replace("%%", playerName);
    }

    public void ChangeAffinity(string target, int delta)
    {
        string normalizedTarget =
            target == null
                ? ""
                : target.Trim().ToLowerInvariant();

        int currentValue;

        switch (normalizedTarget)
        {
            case "architect":
                architectAffinity += delta;
                currentValue = architectAffinity;
                break;

            case "cain":
                cainAffinity += delta;
                currentValue = cainAffinity;
                break;

            case "noah":
                noahAffinity += delta;
                currentValue = noahAffinity;
                break;

            default:
                Debug.LogWarning(
                    $"알 수 없는 호감도 대상입니다: {target}"
                );
                return;
        }

        string deltaText =
            delta > 0
                ? $"+{delta}"
                : delta.ToString();

        Debug.Log(
            $"호감도 변경 | {normalizedTarget}: " +
            $"{deltaText} / 현재 {currentValue}"
        );
    }

    public int GetAffinity(string target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            return 0;
        }

        switch (target.Trim().ToLowerInvariant())
        {
            case "architect":
                return architectAffinity;

            case "cain":
                return cainAffinity;

            case "noah":
                return noahAffinity;

            default:
                return 0;
        }
    }

    /// 아이템을 인벤토리에 추가한 직후 호출합니다.
    /// 해당 아이템이 히든 엔딩 대상이라면 중복 없이 획득 기록합니다.
    public void RegisterHiddenEndingItem(int itemId)
    {
        if (!IsHiddenEndingItem(itemId))
        {
            return;
        }

        // 최초 획득이면 true, 이미 등록된 ID면 false
        bool isNewItem =
            acquiredHiddenEndingItemIds.Add(itemId);

        if (isNewItem)
        {
            Debug.Log(
                $"[Hidden Ending] 히든 아이템 획득: {itemId} " +
                $"({AcquiredHiddenEndingItemCount}/" +
                $"{hiddenEndingItemIds.Length})"
            );
        }
        else
        {
            Debug.Log(
                $"[Hidden Ending] 이미 기록된 히든 아이템입니다: {itemId}"
            );
        }
    }


    /// Noa 히든 엔딩에 필요한 아이템 5개를 모두 획득했는지 확인합니다.
    public bool HasAllHiddenEndingItems()
    {
        if (hiddenEndingItemIds == null ||
            hiddenEndingItemIds.Length != 5)
        {
            Debug.LogWarning(
                "[Hidden Ending] hiddenEndingItemIds에 " +
                "히든 아이템 ID 5개를 정확히 설정해야 합니다."
            );

            return false;
        }

        foreach (int itemId in hiddenEndingItemIds)
        {
            if (!acquiredHiddenEndingItemIds.Contains(itemId))
            {
                return false;
            }
        }

        return true;
    }

    /// 특정 아이템이 Noa 히든 엔딩 대상 아이템인지 확인합니다.
    private bool IsHiddenEndingItem(int itemId)
    {
        if (hiddenEndingItemIds == null)
        {
            return false;
        }

        foreach (int hiddenItemId in hiddenEndingItemIds)
        {
            if (hiddenItemId == itemId)
            {
                return true;
            }
        }

        return false;
    }

    public void ResetGameState()
    {
        playerName = "";
        architectAffinity = 0;
        cainAffinity = 0;
        noahAffinity = 0;

        selectedFaction = FactionType.None;

        // 새 게임 시작 시 히든 아이템 획득 기록도 초기화
        acquiredHiddenEndingItemIds.Clear();
    }

    public void SetSelectedFaction(FactionType faction)
    {
        selectedFaction = faction;

        Debug.Log($"[GameState] 선택된 진영: {selectedFaction}");
    }

}
