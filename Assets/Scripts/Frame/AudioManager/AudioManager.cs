using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region 单例
    private static AudioManager _instace;
    public static AudioManager Instance
    {
        get
        {
            if (_instace == null)
            {
                _instace = FindObjectOfType<AudioManager>();
                if (_instace == null)
                {
                    cameraObj = GameObject.FindObjectOfType<AudioListener>().gameObject;
                    if (cameraObj != null)
                        _instace = cameraObj.AddComponent<AudioManager>();
                    else
                        _instace = new GameObject("AudioManager").AddComponent<AudioManager>();
                }
            }
            return _instace;
        }
    }
    #endregion

    #region 控制的组件
    AudioSource bgmSource;
    SourceManager source;
    ClipManager clip;
    static GameObject cameraObj;
    #endregion

    #region 初始化
    private void Awake()
    {
        if (source == null) source = new SourceManager(cameraObj);
        if (clip == null) clip = new ClipManager();
        InvokeRepeating("ClearFreeSource", 5, 5);
    }

    public void LoadAudios(string path)
    {
        if (clip == null) clip = new ClipManager();
        clip.LoadAudios(path);
    }
    #endregion

    #region 音量控制面板
    //总音量
    private float masterVolume = 1;
    public float MasterVolume
    {
        get { return masterVolume; }
        set
        {
            masterVolume = Mathf.Clamp(value, 0, 1f);
            if (source == null || clip == null) return;
            AudioListener.volume = masterVolume;
        }
    }

    //背景音乐音量
    private float bgmVolume = 1;
    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = Mathf.Clamp(value, 0, 1);
            if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
            if (bgmSource.loop == false) bgmSource.loop = true;
            bgmSource.volume = bgmVolume;
        }
    }

    //音效音量
    private float audioVolume = 1;
    public float AudioVolume
    {
        get
        {
            return audioVolume;
        }

        set
        {
            audioVolume = Mathf.Clamp(value, 0, 1);
            source.SetVolume(audioVolume);
        }
    }
    #endregion

    #region 功能接口
    /// <summary>
    /// 播放一次音频
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayAudio(string audioName, GameObject owner=null, bool loop = false)
    {
        if (string.IsNullOrEmpty(audioName)) return;
        AudioSource temp = source.GetFreeSource(owner);
        temp.loop = loop;
        temp.volume = audioVolume;
        AudioClip tempClip = clip.GetClip(audioName);
        temp.clip = tempClip;
        temp.Play();
    }

    /// <summary>
    /// 循环播放音乐
    /// </summary>
    /// <param name="bgmName"></param>
    public void PlayBGM(string bgmName, bool beForce = false)
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.volume = bgmVolume;
        }
        if (bgmSource.loop == false) bgmSource.loop = true;
        AudioClip tempClip = clip.GetClip(bgmName);
        if (bgmSource.clip == null)
        {
            bgmSource.clip = tempClip;
            bgmSource.Play();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(BgmChange(tempClip, beForce));
        }
    }
    IEnumerator BgmChange(AudioClip newClip, bool beForce = false)
    {
        if (beForce)
        {
            bgmSource.clip = newClip;
            bgmSource.Play();
        }
        else
        {
            float vol = BgmVolume;
            while (bgmSource.volume > 0)
            {
                BgmVolume -= Time.unscaledDeltaTime * 2f;
                yield return null;
            }
            bgmSource.clip = newClip;
            bgmSource.Play();
            while (bgmSource.volume < vol)
            {
                BgmVolume += Time.unscaledDeltaTime * 2f;
                yield return null;
            }
        }
    }

    /// <summary>
    /// 清除所有音频
    /// </summary>
    public void StopAllAudio()
    {
        source.ClearAllSource();
        bgmSource.clip=null;
    }
    /// <summary>
    /// 清除指定音频
    /// </summary>
    /// <param name="audioName"></param>
    public void StopAudio(string audioName)
    {
        source.ClearSource(audioName);
    }
    #endregion

    #region 其它功能
    void ClearFreeSource()
    {
        source.ClearFreeSource();
    }
    #endregion
}
