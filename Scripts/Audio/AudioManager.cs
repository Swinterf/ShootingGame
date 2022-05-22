using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistenSingleten<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;
    const float MIN_Pitch = 0.9f;
    const float MAX_Pitch = 1.1f;

    /// <summary>
    /// ����������Ч�ĺ����������ڲ���Ҫ������ŵĺ�����
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayeSFX(AudioData audioData)
    {
        //sFXPlayer.clip = audioClip;
        //sFXPlayer.volume = volume;
        //sFXPlayer.Play();   //�ú������ܲ��Ÿ�������Ƶ���ᵼ����Ч���ֱ����ϵĸо�
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// ����������������������Ч�ĺ������ʺ�����Ҫ�������ŵ���Ч��
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_Pitch, MAX_Pitch);
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// ��һ����Ч���������һ����Ч�ĺ���
    /// </summary>
    /// <param name="audioDatas"></param>
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }
}
