using UnityEngine;

public class PlayerSFXController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// 발소리 효과음 재생 - 애니메이션 이벤트에서 호출
    /// </summary>
    public void PlayFootStep()
    {
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(ESfx.walking);
    }

    /// <summary>
    /// 점프 효과음 재생
    /// </summary>
    public void PlayJump()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(ESfx.jump);
    }

    /// <summary>
    /// 슬라이드 효과음 재생
    /// </summary>
    public void PlaySlide()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(ESfx.slide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
