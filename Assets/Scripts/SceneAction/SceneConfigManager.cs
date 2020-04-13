﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfigManager : MonoBehaviour
{
    [Header("摄像机是否水平跟随")]
    public bool Horizontal = true;
    [Header("摄像机是否垂直跟随")]
    public bool Vertical = true;

    [Header("移动特效名")]
    public string MoveEffectName;
    [Header("移动特效间隔时间")]
    public float MoveEffectInterval;
    [Header("跳跃落地特效名")]
    public string JumpDownEffectName;

    PolygonCollider2D _collider;

    public static SceneConfigManager Instance;

    float _moveTimeCount;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _moveTimeCount += Time.deltaTime;
    }

    private void Start()
    {
        _collider = GetComponent<PolygonCollider2D>();

        CameraManager.Instance.SetCameraConfiner(_collider);
        CameraManager.Instance.SetCameraFollowMode(Horizontal, Vertical);
    }

    public void PlayMoveEffect(Character owner)
    {
        if (string.IsNullOrEmpty(MoveEffectName)) return;
        var hit = Physics2D.Raycast(owner.transform.position, Vector2.down, 3, GameConfig.Instance.Plane);
        if (hit)
        {
            if (_moveTimeCount > MoveEffectInterval)
            {
                _moveTimeCount = 0;
                EffectManager.Instance.Play(MoveEffectName, hit.point, true);
            }
        }
    }

    public void PlayJumpDownEffect(Character owner)
    {
        if (string.IsNullOrEmpty(JumpDownEffectName)) return;
        var hit = Physics2D.Raycast(owner.transform.position, Vector2.down, 3, GameConfig.Instance.Plane);
        if (hit)
        {
            EffectManager.Instance.Play(JumpDownEffectName, hit.point, true);
        }
    }
}
