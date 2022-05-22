using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistenSingleten<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;
    const float MIN_Pitch = 0.9f;
    const float MAX_Pitch = 1.1f;

    /// <summary>
    /// 用来播放音效的函数（适用于不需要随机播放的函数）
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayeSFX(AudioData audioData)
    {
        //sFXPlayer.clip = audioClip;
        //sFXPlayer.volume = volume;
        //sFXPlayer.Play();   //该函数不能播放复数段音频，会导致音效有种被掐断的感觉
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// 用来播放音高随机的随机音效的函数（适合于需要连续播放的音效）
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_Pitch, MAX_Pitch);
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// 从一组音效中随机播放一个音效的函数
    /// </summary>
    /// <param name="audioDatas"></param>
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }
}
