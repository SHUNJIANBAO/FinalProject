using UnityEngine;

public class ClipManager
{
    AudioClip[] objs;
    public void LoadAudios(string path)
    {
        objs = Resources.LoadAll<AudioClip>(path);
    }

    public AudioClip GetClip(string clipName)
    {
        foreach (AudioClip item in objs)
        {
            if (item.name == clipName)
            {
                return item;
            }
        }
        Debug.LogError("没有名称为：" + clipName + "的音频");
        return null;
    }
}
