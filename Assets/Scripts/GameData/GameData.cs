﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioVolumeInfo
{
    public float BGMVoulume = 1;
    public float AudioVolume = 1;
    public float MasterVolume = 1;
}
[System.Serializable]
public class InputKeyInfo
{
    public E_InputKey m_Key;
    public KeyCode Key;
}

public class GameData : Data<GameData>
{
    public AudioVolumeInfo AudioVolumeInfo;
    public List<InputKeyInfo> KeyCodeList;

    Dictionary<E_InputKey, KeyCode> _keyDict;

    protected override void OnLoad()
    {
        base.OnLoad();
        if (AudioVolumeInfo != null)
        {
            AudioManager.Instance.MasterVolume = AudioVolumeInfo.MasterVolume;
            AudioManager.Instance.BGMVolume = AudioVolumeInfo.BGMVoulume;
            AudioManager.Instance.AudioVolume = AudioVolumeInfo.AudioVolume;
        }
        else
        {
            AudioVolumeInfo = new AudioVolumeInfo();
        }

        if (KeyCodeList == null)
        {
            ResetKey();
        }
        else
        {
            _keyDict = new Dictionary<E_InputKey, KeyCode>();
            for (int i = 0; i < KeyCodeList.Count; i++)
            {
                _keyDict.Add(KeyCodeList[i].m_Key, KeyCodeList[i].Key);
            }
        }
    }

    public void SetAudioVolume(E_AudioType type, float value)
    {
        switch (type)
        {
            case E_AudioType.Master:
                AudioVolumeInfo.MasterVolume = value;
                break;
            case E_AudioType.Music:
                AudioVolumeInfo.BGMVoulume = value;
                break;
            case E_AudioType.Audio:
                AudioVolumeInfo.AudioVolume = value;
                break;
        }
    }

    public void SetKey(E_InputKey mKey, KeyCode curKey)
    {
        KeyCodeList.Find(k => k.m_Key == mKey).Key = curKey;
    }

    public KeyCode GetKey(E_InputKey mKey)
    {
        return _keyDict[mKey];
    }

    public void ResetKey()
    {
        KeyCodeList = new List<InputKeyInfo>();
        InputKeyInfo info = new InputKeyInfo();
        info.m_Key = E_InputKey.Up;
        info.Key = KeyCode.W;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Down;
        info.Key = KeyCode.S;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Left;
        info.Key = KeyCode.A;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Rigth;
        info.Key = KeyCode.D;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Attack;
        info.Key = KeyCode.J;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.LongAttack;
        info.Key = KeyCode.U;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Jump;
        info.Key = KeyCode.K;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Blink;
        info.Key = KeyCode.L;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Skill;
        info.Key = KeyCode.I;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Support;
        info.Key = KeyCode.O;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Bag;
        info.Key = KeyCode.B;
        KeyCodeList.Add(info);
        info = new InputKeyInfo();
        info.m_Key = E_InputKey.Map;
        info.Key = KeyCode.M;
        KeyCodeList.Add(info);

        _keyDict = new Dictionary<E_InputKey, KeyCode>();
        for (int i = 0; i < KeyCodeList.Count; i++)
        {
            _keyDict.Add(KeyCodeList[i].m_Key, KeyCodeList[i].Key);
        }
    }
}

public enum E_AudioType
{
    Master,
    Music,
    Audio
}

public enum E_InputKey
{
    Left,
    Rigth,
    Up,
    Down,
    Attack,
    LongAttack,
    Jump,
    Blink,
    Skill,
    Support,
    Bag,
    Map,
}
