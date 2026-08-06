using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    [Header("재생할 BGM")]
    [SerializeField] private EBgm bgm;

    void Start()
    {
        if(SoundManager.Instance == null)
        {
            Debug.LogWarning("사운드 매니저가 현재 씬에 없습니다.");
            return;
        }
        SoundManager.Instance.PlayBGM(bgm);
    }
}
