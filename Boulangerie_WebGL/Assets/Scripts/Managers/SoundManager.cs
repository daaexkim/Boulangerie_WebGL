using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SFXType { Pop, Drop, Button, Over, Fire, Coin };

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public BGMClip[] bGMClips;
    public SFXClip[] sfxClips;

    private int sfxCursor;

    private void Start()
    {
        BGMPlay(3);
    }

    public void SFXPlay(SFXType type)
    {
        SFXClip clip = FindSFXClip(type);
        AudioClip[] clips = clip.sfxClip;

        sfxPlayer[sfxCursor].clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        sfxPlayer[sfxCursor].volume = clip.volume;

        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }
    public void BGMPlay(int id)
    {
        bgmPlayer.clip = bGMClips[id].bgmClip;
        bgmPlayer.volume = Mathf.Clamp01(bGMClips[id].volume);
        bgmPlayer.Play();
    }
    private SFXClip FindSFXClip(SFXType type)
    {
        return Array.Find(sfxClips, clip => clip.type == type);
    }
}

[Serializable]
public struct SFXClip
{
    public SFXType type;
    public float volume;
    public AudioClip[] sfxClip;
}
[Serializable]
public struct BGMClip
{
    public float volume;
    public AudioClip bgmClip;
}