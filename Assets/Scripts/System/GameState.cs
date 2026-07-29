using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private string playerName = "";

    [Header("Affinity")]
    [SerializeField] private int architectAffinity;
    [SerializeField] private int cainAffinity;
    [SerializeField] private int noahAffinity;

    public string PlayerName => playerName;

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

    public void ResetGameState()
    {
        playerName = "";
        architectAffinity = 0;
        cainAffinity = 0;
        noahAffinity = 0;
    }
}
