using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    string AudioSettingKey = "Audio";
    protected AudioManager()
    {
    }
    public void PlayClip(AudioSource audioSource)
    {
        if (!PlayerPrefs.HasKey(AudioSettingKey)){
            PlayerPrefs.SetInt(AudioSettingKey,1);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt(AudioSettingKey) == 1){
            audioSource.Play();
        }

    }
    public void SetAudio(bool audioOn)
    {
        int audioSetting = audioOn ? 1 : 0;
        PlayerPrefs.SetInt(AudioSettingKey, audioSetting);
        PlayerPrefs.Save();
    }

}
