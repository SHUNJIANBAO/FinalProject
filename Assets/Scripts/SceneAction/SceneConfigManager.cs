using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneConfigManager : MonoBehaviour
{
    public int LevelId;

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

    Renderer[] _renderArray;
    Material _default;

    float _moveTimeCount;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init(LevelId);
        CloseTrashCamera();
        _collider = this.GetComponent<PolygonCollider2D>();
        _renderArray = this.GetComponentsInChildren<Renderer>();
        _default = _renderArray[0].material;
        GameManager.CurSceneManager = this;

        CameraManager.SetCameraConfiner(this._collider);
        CameraManager.SetCameraFollowMode(Horizontal, Vertical);
    }

    void Init(int levelId)
    {

    }

    public void SetSceneGray(bool value)
    {
        if (value)
        {
            for (int i = 0; i < _renderArray.Length; i++)
            {
                _renderArray[i].material = ResourceManager.Load<Material>("Materials/Gray");
            }
        }
        else
        {
            for (int i = 0; i < _renderArray.Length; i++)
            {
                _renderArray[i].material = _default;
            }
        }
    }

    public void PlayMoveEffect(Character owner)
    {
        if (string.IsNullOrEmpty(MoveEffectName)) return;
        var hit = Physics2D.Raycast(owner.transform.position, Vector2.down, 3, GameConfig.Instance.PlaneMask);
        if (hit)
        {
            if (_moveTimeCount > MoveEffectInterval)
            {
                _moveTimeCount = 0;
                EffectManager.Instance.Play(MoveEffectName, hit.point, owner.transform.localScale.x > 0);
            }
        }
    }

    public void PlayJumpDownEffect(Character owner)
    {
        if (string.IsNullOrEmpty(JumpDownEffectName)) return;
        var hit = Physics2D.Raycast(owner.transform.position, Vector2.down, 3, GameConfig.Instance.PlaneMask);
        if (hit)
        {
            EffectManager.Instance.Play(JumpDownEffectName, hit.point, owner.transform.localScale.x > 0);
        }
    }

    void CloseTrashCamera()
    {
        var cams = GameObject.FindObjectsOfType<Camera>();
        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i].tag == "MainCamera" && !CameraManager.IsMainCamera(cams[i]))
                cams[i].gameObject.SetActive(false);
        }
    }
}
