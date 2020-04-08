using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIArchiveWindow : UIWindowBase
{
    #region 参数
    Transform Panel_ArchiveRoot;

    Button Button_DeletePlayerInfo;
    Button Button_CancelDelete;
    GameObject Panel_CheckDeletePlayerInfo;


    int _curIndex;
    PlayerInfo _curInfo;
    List<UIArchivePanel> _archiveList;
    #endregion

    #region 继承方法

    /// <summary>
    /// 得到UI组件
    /// </summary>
    protected override void GetUIComponent()
    {
        base.GetUIComponent();
        Panel_ArchiveRoot = GetUI<Transform>("Panel_ArchiveRoot");


        Button_DeletePlayerInfo = GetUI<Button>("Button_DeletePlayerInfo");
        Button_CancelDelete = GetUI<Button>("Button_CancelDelete");
        Panel_CheckDeletePlayerInfo = GetUI<GameObject>("Panel_CheckDeletePlayerInfo");
        _archiveList = GetUIList<UIArchivePanel>("UIArchivePanel");
    }

    /// <summary>
    /// 给UI添加方法
    /// </summary>
    protected override void AddUIListener()
    {
        base.AddUIListener();
        AddButtonListen(Button_CancelDelete, CloseCheckWindow);
        AddButtonListen(Button_DeletePlayerInfo, OnClickButtonDeletePlayerInfo);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();
        CloseCheckWindow();
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
        SetArchiveList(PlayerData.Instance.PlayerInfos);
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
    void SetArchiveList(List<PlayerInfo> infos)
    {
        for (int i = 0; i < _archiveList.Count; i++)
        {
            var info = infos.Find(s => s.SaveId == i);
            _archiveList[i].Init(i, info);
        }
    }

    public void EnterGame(PlayerInfo info,bool isNewGame)
    {
        PlayerData.Instance.CurPlayerInfo = info;
        System.Action callback = BattleSceneStatus.Instance.StartBattle;
        if (isNewGame)
        {
            callback +=()=> PlayerData.Instance.SavePlayerInfo(info);
        }
        LoadSceneManager.Instance.LoadSceneAsync(info.CurLevelId, callback);
    }

    public void OpenDeletePlayerInfoPanel(PlayerInfo info)
    {
        _curInfo = info;
        Panel_CheckDeletePlayerInfo.SetActive(true);
    }

    void OnClickButtonDeletePlayerInfo()
    {
        PlayerData.Instance.DeletePlayerInfo(_curInfo);
        SetArchiveList(PlayerData.Instance.PlayerInfos);
        CloseCheckWindow();
    }

    void CloseCheckWindow()
    {
        Panel_CheckDeletePlayerInfo.SetActive(false);
    }

    #endregion
}
