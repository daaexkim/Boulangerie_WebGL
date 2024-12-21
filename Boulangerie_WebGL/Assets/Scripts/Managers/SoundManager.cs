using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SFXType { Pop, Drop, Button, Over, Fire, Coin };

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public BGMClip[] bgmClips;
    public SFXClip[] sfxClips;

    private int sfxCursor;

    public void SFXPlay(SFXType type)
    {
        SFXClip clip = FindSFXClip(type);
        AudioClip[] clips = clip.sfxClip;

        sfxPlayer[sfxCursor].clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        sfxPlayer[sfxCursor].volume = clip.volume;

        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }
    //public void BGMPlay(int id)
    //{
    //    BGMClip clip = FindBGMClip(id);

    //    bgmPlayer.clip = null;
    //    bgmPlayer.clip = clip.bgmClip;
    //    bgmPlayer.volume = Mathf.Clamp01(clip.volume);
    //    bgmPlayer.Play();
    //}
    private SFXClip FindSFXClip(SFXType type)
    {
        return Array.Find(sfxClips, clip => clip.type == type);
    }
    private BGMClip FindBGMClip(int id)
    {
        return Array.Find(bgmClips, clip => clip.id == id);
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
    public int id;
    public float volume;
    public AudioClip bgmClip;
}