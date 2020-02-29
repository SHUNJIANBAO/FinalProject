using System.Collections.Generic;
using UnityEngine;

public class SourceManager
{
    #region 变量
    //摄像机
    GameObject cameraObj;
    List<AudioSource> sourceList;
    int sourceMaxCount;
    #endregion

    public SourceManager(GameObject cameraObj, int sourceMaxCount = 3)
    {
        this.cameraObj = cameraObj;
        this.sourceMaxCount = sourceMaxCount;
        OnInit();
    }

    void OnInit()
    {
        sourceList = new List<AudioSource>();
        for (int i = 0; i < sourceMaxCount; i++)
        {
            AudioSource source = cameraObj.AddComponent<AudioSource>();
            sourceList.Add(source);
        }
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        foreach (AudioSource temp in sourceList)
        {
            temp.volume = volume;
        }
    }

    /// <summary>
    /// 得到一个空闲的音频播放器
    /// </summary>
    /// <returns></returns>
    public AudioSource GetFreeSource(GameObject owner = null)
    {
        if (owner == null) owner = cameraObj;
        foreach (AudioSource source in sourceList)
        {
            if (source.isPlaying == false)
            {
                source.clip = null;
            }
            if (source.clip == null && source.gameObject == owner)
            {
                return source;
            }
        }
        AudioSource temp = owner.AddComponent<AudioSource>();
        if (cameraObj != owner)
            temp.spatialBlend = 1;
        sourceList.Add(temp);
        return temp;
    }

    List<AudioSource> tempList;
    /// <summary>
    /// 清除多余的音频播放器
    /// </summary>
    public void ClearFreeSource()
    {
        if (sourceList.Count < sourceMaxCount) return;
        if (tempList == null) tempList = new List<AudioSource>();
        int count = 0;
        for (int i = 0; i < sourceList.Count; i++)
        {
            if (sourceList[i].isPlaying == false)
            {
                sourceList[i].clip = null;
                count++;
                if (count > 3)
                {
                    tempList.Add(sourceList[i]);
                }
            }
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            sourceList.Remove(tempList[i]);
            GameObject.Destroy(tempList[i]);
        }
        tempList.Clear();
    }

    public void ClearSource(string clipName)
    {
        foreach (AudioSource temp in sourceList)
        {
            if (temp.clip == null) continue;
            if (temp.clip.name == clipName)
            {
                temp.clip = null;
            }
        }
    }

    public void ClearAllSource()
    {
        if (sourceList.Count == 0) return;
        foreach (AudioSource temp in sourceList)
        {
            if (temp.clip == null) continue;
            temp.clip = null;
            temp.loop = false;
        }
    }
}
