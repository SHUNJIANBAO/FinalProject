using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArchivePanel : UIPanelBase
{
    Text Text_PlayTimeLong;
    Button Button_DeleteArchive;
    Button Button_ClickArchive;
    Image Image_Level;
    Text Text_LevelName;
    GameObject GameObject_NullArchive;

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
        Image_Level = GetUI<Image>("Image_Level");
        Text_LevelName = GetUI<Text>("Text_LevelName");
        GameObject_NullArchive = GetUI<GameObject>("GameObject_NullArchive");
    }

    public void Init(int index, PlayerInfo info)
    {
        Button_DeleteArchive.onClick.RemoveAllListeners();
        Button_ClickArchive.onClick.RemoveAllListeners();
        _index = index;
        _info = info;
        if (info == null)
        {
            Button_DeleteArchive.gameObject.SetActive(false);
            Text_LevelName.text = "";
            Image_Level.sprite = null;
            Text_PlayTimeLong.text = "";
            GameObject_NullArchive.SetActive(true);
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
                Text_PlayTimeLong.text = hours + "h:" + minutes + "m";
            }
            else
            {
                Text_PlayTimeLong.text = _info.GameTimeMinutes + "m";
            }
            LevelConfig levelCfg = LevelConfig.GetData(info.CurLevelId);
            Image_Level.sprite = ResourceManager.Load<Sprite>("Sprites/" + levelCfg.TitleIcon);
            Text_LevelName.text = levelCfg.Name;
            GameObject_NullArchive.SetActive(false);
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
            UIArchiveWindow.EnterGame(_info, true);
        }
        else
        {
            UIArchiveWindow.EnterGame(_info, false);
        }
    }
}
