using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool playOnStart;

    [Header("재생할 BGM")]
    [SerializeField] private EBgm bgm;

    void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play() // 인스펙터의 UnityEvent에서 연결할 수 있도록 public으로 분리
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("사운드 매니저가 현재 씬에 없습니다.");
            return;
        }

        SoundManager.Instance.PlayBGM(bgm);
    }
}

