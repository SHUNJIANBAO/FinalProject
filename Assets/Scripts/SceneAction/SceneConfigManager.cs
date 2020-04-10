using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfigManager : MonoBehaviour
{
    [Header("摄像机是否水平跟随")]
    public bool Horizontal = true;
    [Header("摄像机是否垂直跟随")]
    public bool Vertical = true;

    PolygonCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<PolygonCollider2D>();

        CameraManager.Instance.SetCameraConfiner(_collider);
        CameraManager.Instance.SetCameraFollowMode(Horizontal, Vertical);
    }
}
