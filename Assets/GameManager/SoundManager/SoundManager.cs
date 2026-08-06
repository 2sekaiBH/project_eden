using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum EBgm
{
    Dialogue_67 = 0,
    Dialogue_218,
    Dialogue_399,
    Dialogue_404,
    CardGame,
    CardGame_404,
    Ending_Arch,
    Ending_Eve,
    Ending_Noa,
    Neon_Static,
    Prologue
}

public enum ESfx
{

}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    private Dictionary<EBgm, AudioClip> bgmDict = new Dictionary<EBgm, AudioClip>();
    private Dictionary<ESfx, AudioClip> sfxDict = new Dictionary<ESfx, AudioClip>();

    [Header("Reference")]
    [SerializeField] private AudioSource sfxPlayer; // SFX 재생용 AudioSource
    [SerializeField] private AudioSource bgmPlayer; // BGM 재생용 AudioSource
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] sfxClips;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    private void Init()
    {
        // bgm dictionary 초기화
        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgmDict[(EBgm)i] = bgmClips[i];
        }

        // sfx dictionary 초기화
        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxDict[(ESfx)i] = sfxClips[i];
        }
    }
    
    public void PlayBGM(EBgm bgmType)
    {
        if(bgmDict.TryGetValue(bgmType, out var clip))
        {
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Bgm not found in Dictionary!");
        }
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(ESfx sfxType)
    {
        if(sfxDict.TryGetValue(sfxType, out var clip))
        {
            sfxPlayer.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found in Dictionary");
        }
    }

    public void SetBgmVolume(float volume)
    {
        mixer.SetFloat("BgmVolumeParam", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    public void SetSfxVolume(float volume)
    {
        mixer.SetFloat("SfxVolumeParam", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    public float GetBgmVolume()
    {
        if (mixer.GetFloat("BgmVolumeParam", out float dB))
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        return 1f;
    }

    public float GetSfxVolume()
    {
        if (mixer.GetFloat("SfxVolumeParam", out float dB))
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        return 1f;
    }
}
