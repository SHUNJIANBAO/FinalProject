using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UISetWindow : UIWindowBase
{
    #region 参数
    Slider Slider_MasterVolume;
    Slider Slider_MusicVolume;
    Slider Slider_AudioVolume;
    #endregion

    #region 继承方法
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();
        ShowVolumes(GameData.Instance.AudioVolumeInfo);
    }

    /// <summary>
    /// 得到UI组件
    /// </summary>
    protected override void GetUIComponent()
    {
        base.GetUIComponent();
        Slider_MasterVolume = GetUI<Slider>("Slider_MasterVolume");
        Slider_MusicVolume = GetUI<Slider>("Slider_MusicVolume");
        Slider_AudioVolume = GetUI<Slider>("Slider_AudioVolume");
    }

    /// <summary>
    /// 给UI添加方法
    /// </summary>
    protected override void AddUIListener()
    {
        base.AddUIListener();
        AddSliderListen(Slider_MasterVolume, OnSliderChangeMasterVolume);
        AddSliderListen(Slider_MusicVolume, OnSliderChangeMusicVolume);
        AddSliderListen(Slider_AudioVolume, OnSliderChangeAudioVolume);
    }

    /// <summary>
    /// 刷新信息，需要手动调用该方法
    /// </summary>
    /// <param name="objs"></param>
    public override void OnRefresh(params object[] objs)
    {
        base.OnRefresh(objs);
    }

    /// <summary>
    /// 打开界面时调用
    /// </summary>
    /// <param name="objs"></param>
    public override void OnOpen(params object[] objs)
    {
        base.OnOpen(objs);
        SetKeys(GameData.Instance.KeyCodeList);
    }

    /// <summary>
    /// 关闭界面时调用
    /// </summary>
    /// <param name="objs"></param>
    public override void OnClose(params object[] objs)
    {
        base.OnClose(objs);
        GameData.Save();
    }

    /// <summary>
    /// 打开时的动画效果
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override IEnumerator StartOpenAnim(UICallBack uiCallBack, params object[] objs)
    {
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            StartCoroutine(base.StartOpenAnim(uiCallBack, objs));
        });
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// 关闭时的动画效果,
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override IEnumerator StartCloseAnim(UICallBack uiCallBack, params object[] objs)
    {
        m_CanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            StartCoroutine(base.StartCloseAnim(uiCallBack, objs));
        });
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region 成员方法

    void SetKeys(List<InputKeyInfo> keyCodeList)
    {

    }

    void ShowVolumes(AudioVolumeInfo info)
    {
        Slider_MasterVolume.value = info.MasterVolume;
        Slider_MusicVolume.value = info.BGMVoulume;
        Slider_AudioVolume.value = info.AudioVolume;
    }

    public void OnClickButtonResetKey()
    {
        GameData.Instance.ResetKey();
        SetKeys(GameData.Instance.KeyCodeList);
    }

    public void OnSliderChangeMasterVolume(float value)
    {
        GameData.Instance.SetAudioVolume(E_AudioType.Master, value);
        AudioManager.Instance.MasterVolume = value;
    }
    public void OnSliderChangeMusicVolume(float value)
    {
        GameData.Instance.SetAudioVolume(E_AudioType.Music, value);
        AudioManager.Instance.BGMVolume = value;
    }
    public void OnSliderChangeAudioVolume(float value)
    {
        GameData.Instance.SetAudioVolume(E_AudioType.Audio, value);
        AudioManager.Instance.AudioVolume = value;
    }
    #endregion
}
