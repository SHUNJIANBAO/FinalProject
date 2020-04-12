using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoSingleton<CameraManager>
{
    Camera _mainCamera;
    CinemachineConfiner _cameraConfiner;
    CinemachineVirtualCamera _cinemachine;
    CinemachineFramingTransposer _body;


    private void Start()
    {
        _mainCamera = GetComponentInChildren<Camera>();
        _cameraConfiner = GetComponentInChildren<CinemachineConfiner>();
        _cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
        foreach (var script in _cinemachine.GetComponentPipeline())
        {
            if (script is CinemachineFramingTransposer)
            {
                _body = script as CinemachineFramingTransposer;
                break;
            }
        }
        if (_body==null)
        {
            Debug.LogError("Body Con't Find!!!");
        }
    }

    public bool IsMainCamera(Camera cam)
    {
        return _mainCamera == cam;
    }

    /// <summary>
    /// 设置摄像机活动范围
    /// </summary>
    /// <param name="collide"></param>
    public void SetCameraConfiner(PolygonCollider2D collide)
    {
        _cameraConfiner.m_BoundingShape2D = collide;
    }

    /// <summary>
    /// 设置跟随模式
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void SetCameraFollowMode(bool horizontal,bool vertical)
    {
        if (!horizontal)
        {
            _body.m_SoftZoneWidth = 2;
            _body.m_DeadZoneWidth = 1;
        }
        else
        {
            _body.m_SoftZoneWidth = 0.05f;
        }
        if (!vertical)
        {
            _body.m_SoftZoneHeight = 2;
            _body.m_DeadZoneHeight = 1;
        }
        else
        {
            _body.m_SoftZoneHeight = 0.05f;
        }
    }

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    /// <param name="target"></param>
    public void SetFollowTarget(Transform target)
    {
        _cinemachine.m_Follow = target;
    }
}
