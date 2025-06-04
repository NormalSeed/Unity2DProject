using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public enum EBgm
    {
        BGM_TITLE,
        BGM_STAGE
    }

    public enum ESfx
    {
        SFX_GUNSHOT
    }

    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    [SerializeField] public AudioSource audioBgm;
    [SerializeField] public AudioSource audioSfx;

    private void Awake() => Init();

    private void Init()
    {
        base.SingletonInit();
    }

    public void PlayBGM(EBgm bgm)
    {
        audioBgm.clip = bgms[(int)bgm];
        audioBgm.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }

    public void PlaySFX(ESfx sfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)sfx]);
    }
}
