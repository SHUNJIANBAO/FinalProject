using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioVolumeInfo
{
    public float MusicVoulume=1;
    public float AudioVolume=1;
    public float MasterVolume=1;
}

public class GameData : Data<GameData>
{
    public AudioVolumeInfo AudioVolumeInfo;

    protected override void OnLoad()
    {
        base.OnLoad();
        if (AudioVolumeInfo!=null)
        {
            AudioManager.Instance.MasterVolume = AudioVolumeInfo.MasterVolume;
            AudioManager.Instance.MusicVolume = AudioVolumeInfo.MusicVoulume;
            AudioManager.Instance.AudioVolume = AudioVolumeInfo.AudioVolume;
        }
    }
}
