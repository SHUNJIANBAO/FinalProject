using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UILoadingWindow : UIWindowBase
{
    #region 参数
    Text Text_LoadProgress;
    Image Image_LoadProgress;

    [HideInInspector]
    public float Progress { get; private set; }
    float m_Progress;
    #endregion

    #region 继承方法

    /// <summary>
    /// 得到UI组件
    /// </summary>
    protected override void GetUIComponent()
    {
        base.GetUIComponent();
        Text_LoadProgress = GetUI<Text>("Text_LoadProgress");
        Image_LoadProgress = GetUI<Image>("Image_LoadProgress");
    }

    /// <summary>
    /// 给UI添加方法
    /// </summary>
    protected override void AddUIListener()
    {
        base.AddUIListener();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();
        Text_LoadProgress.text = "0%";
        Image_LoadProgress.fillAmount = 0;
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
        GameManager.IsLoading = true;
    }

    /// <summary>
    /// 关闭界面时调用
    /// </summary>
    /// <param name="objs"></param>
    public override void OnClose(params object[] objs)
    {
        base.OnClose(objs);
        Text_LoadProgress.text = "0%";
        Image_LoadProgress.fillAmount = 0;
        Image_LoadProgress.enabled = false;
        Text_LoadProgress.enabled = false;
        GameManager.IsLoading = false;
        Progress = 0;
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
        m_CanvasGroup.DOFade(0, 0.6f).OnComplete(() =>
        {
            StartCoroutine(base.StartCloseAnim(uiCallBack, objs));
        });
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region 成员方法

    public void SetProgress(float value)
    {
        if (Image_LoadProgress.enabled == false)
        {
            Image_LoadProgress.enabled = true;
            Text_LoadProgress.enabled = true;
        }
        m_Progress = value;
    }
    private void Update()
    {
        if (Image_LoadProgress.enabled)
        {
            if (Image_LoadProgress.fillAmount < m_Progress)
            {
                Image_LoadProgress.fillAmount += Time.deltaTime;
                Progress = Image_LoadProgress.fillAmount;
                Text_LoadProgress.text = string.Format("{0}%", Mathf.CeilToInt(Image_LoadProgress.fillAmount * 100));
            }
        }
    }

    #endregion
}
