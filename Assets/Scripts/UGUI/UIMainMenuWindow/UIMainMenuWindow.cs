using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIMainMenuWindow : UIWindowBase
{
    #region 参数

    Button Button_EnterGame;
    Button Button_ExitGame;

    List<Transform> m_ButtonList = new List<Transform>();

    GameObject Panel_MenuButton;
    Animator Animator_OpenTitle;

    //临时
    CanvasGroup alpha;
    #endregion

    #region 继承方法
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();

        Button_EnterGame = GetUI<Button>("Button_EnterGame");
        Button_ExitGame = GetUI<Button>("Button_ExitGame");

        m_ButtonList.Add(Button_EnterGame.transform);
        m_ButtonList.Add(Button_ExitGame.transform);

        Panel_MenuButton = GetUI<GameObject>("Panel_MenuButton");
        Animator_OpenTitle = GetUI<Animator>("Animator_OpenTitle");

        alpha = GetUI<CanvasGroup>("Animator_OpenTitle");

        //Panel_MenuButton.SetActive(false);
        Animator_OpenTitle.gameObject.SetActive(false);
    }

    /// <summary>
    /// 得到UI组件
    /// </summary>
    protected override void GetUIComponent()
    {
        base.GetUIComponent();
    }

    /// <summary>
    /// 给UI添加方法
    /// </summary>
    protected override void AddUIListener()
    {
        base.AddUIListener();
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
        StartCoroutine(HideButtons(null,0,0));
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
    /// 打开时的动画效果
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override IEnumerator StartOpenAnim(UICallBack uiCallBack, params object[] objs)
    {
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.DOFade(1, 0.4f).OnComplete(() =>
        {
            StartCoroutine(base.StartOpenAnim(uiCallBack, objs));
            ShowTitleAnim();
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
        m_CanvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            StartCoroutine(base.StartCloseAnim(uiCallBack, objs));
        });
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region 成员方法

    private void Update()
    {
        if (Animator_OpenTitle.gameObject.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                HideTitleAnim();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(HideButtons(ShowTitleAnim));
            }
        }
    }

    /// <summary>
    /// 展示标题动画
    /// </summary>
    void ShowTitleAnim()
    {
        alpha.alpha = 0;
        Animator_OpenTitle.gameObject.SetActive(true);
        alpha.DOFade(1, 0.5f);
    }

    void HideTitleAnim()
    {
        alpha.DOFade(0, 0.3f).OnComplete(() =>
        {
            Animator_OpenTitle.gameObject.SetActive(false);
            StartCoroutine(ShowButtons());
        });
    }

    /// <summary>
    /// 显示界面按钮
    /// </summary>
    IEnumerator ShowButtons()
    {
        foreach (var btn in m_ButtonList)
        {
            btn.DOLocalMoveX(0, 0.3f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 隐藏界面按钮
    /// </summary>
    /// <returns></returns>
    IEnumerator HideButtons(System.Action action=null,float intervalTime=0.1f,float moveSpeed=0.3f)
    {
        foreach (var btn in m_ButtonList)
        {
            btn.DOLocalMoveX(600, moveSpeed);
            yield return new WaitForSeconds(intervalTime);
        }
        if (action != null)
            action();
    }

    #endregion
}
