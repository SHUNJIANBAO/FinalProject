using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArchivePanel : UIPanelBase
{
    Text Text_PlayTimeLong;
    Button Button_DeleteArchive;
    Button Button_ClickArchive;
    UIArchiveWindow UIArchiveWindow;
    PlayerInfo _info;
    int _index;
    protected override void GetUIComponent()
    {
        base.GetUIComponent();
        UIArchiveWindow = GetComponentInParent<UIArchiveWindow>();
        Text_PlayTimeLong = GetUI<Text>("Text_PlayTimeLong");
        Button_ClickArchive = GetUI<Button>("Button_ClickArchive");
        Button_DeleteArchive = GetUI<Button>("Button_DeleteArchive");
    }

    public void Init(int index, PlayerInfo info)
    {
        _index = index;
        _info = info;
        if (info == null)
        {
            Button_DeleteArchive.gameObject.SetActive(false);
        }
        else
        {
            if (_index != info.SaveId) throw new System.Exception("存档位置不匹配");
            Button_DeleteArchive.gameObject.SetActive(true);
            AddButtonListen(Button_DeleteArchive, OnClickButtonDeletePlayerInfo);

            int hours = _info.GameTimeMinutes / 60;
            if (hours > 0)
            {
                int minutes = (_info.GameTimeMinutes - hours * 60);
                Text_PlayTimeLong.text = string.Format("{0}h:{1}m", hours, minutes);
            }
            else
            {
                Text_PlayTimeLong.text = string.Format("{0}m", _info.GameTimeMinutes);
            }
        }
        AddButtonListen(Button_ClickArchive, OnClickButtonEnterGame);
    }

    void OnClickButtonDeletePlayerInfo()
    {
        UIArchiveWindow.OpenDeletePlayerInfoPanel(_info);
    }

    void OnClickButtonEnterGame()
    {
        if (_info == null)
        {
            _info = PlayerData.Instance.CreatePlayerInfo(_index);
        }
        UIArchiveWindow.EnterGame(_info);
    }
}
