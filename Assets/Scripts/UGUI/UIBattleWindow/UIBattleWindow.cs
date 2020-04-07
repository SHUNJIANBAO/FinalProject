using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIBattleWindow : UIWindowBase
{
    #region 参数
    static public float ValueChangeDuration = 0.2f;
    static public float UIAnimChangeDuration = 0.2f;

    static Image Image_PlayerHp;
    static Image Image_PlayerMp;
    static Image Image_BossHp;
    static Image Image_BossShield;

    static CanvasGroup Panel_PlayerUI;
    static CanvasGroup Panel_BossUI;



    #endregion

    #region 继承方法

    /// <summary>
    /// 得到UI组件
    /// </summary>
    protected override void GetUIComponent()
    {
        base.GetUIComponent();
        Image_PlayerHp = GetUI<Image>("Image_PlayerHp");
        Image_PlayerMp = GetUI<Image>("Image_PlayerMp");
        Image_BossHp = GetUI<Image>("Image_BossHp");
        Image_BossShield = GetUI<Image>("Image_BossShield");

        Panel_PlayerUI = GetUI<CanvasGroup>("Panel_PlayerUI");
        Panel_BossUI = GetUI<CanvasGroup>("Panel_BossUI");
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
        Panel_BossUI.alpha = 0;
        Panel_BossUI.blocksRaycasts = false;
        Panel_PlayerUI.alpha = 1;
        Panel_PlayerUI.blocksRaycasts = false;
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
    }

    /// <summary>
    /// 关闭界面时调用
    /// </summary>
    /// <param name="objs"></param>
    public override void OnClose(params object[] objs)
    {
        base.OnClose(objs);
    }

    /// <summary>
    /// 获得焦点时
    /// </summary>
    public override void OnFocus()
    {
        base.OnFocus();
    }

    /// <summary>
    /// 失去焦点时
    /// </summary>
    public override void OnLostFocus()
    {
        base.OnLostFocus();
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

    public static void ShowPlayerUI()
    {
        Panel_PlayerUI.DOFade(1, UIAnimChangeDuration);
    }
    public static void HidePlayerUI()
    {
        Panel_PlayerUI.DOFade(0, UIAnimChangeDuration);
    }

    public static void ShowBossUI()
    {
        Panel_BossUI.DOFade(1, UIAnimChangeDuration);
    }
    public static void HideBossUI()
    {
        Panel_BossUI.DOFade(0, UIAnimChangeDuration);
    }


    public static void OnPlayerHpChange(float current,float min,float max)
    {
        Image_PlayerHp.DOFillAmount(current / max, ValueChangeDuration);
    }
    public static void OnPlayerMpChange(float current,float min,float max)
    {
        Image_PlayerMp.DOFillAmount(current / max, ValueChangeDuration);
    }

    public static void OnBossHpChange(float current,float min,float max)
    {
        Image_BossHp.DOFillAmount(current / max, ValueChangeDuration);
    }
    public static void OnBossShieldChange(float current,float min,float max)
    {
        Image_BossShield.DOFillAmount(current / max, ValueChangeDuration);
    }

    #endregion
}
