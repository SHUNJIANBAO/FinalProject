using UnityEngine;

public class PortalPoint : MonoBehaviour
{
    [Header("目标关卡Id")]
    public int TargetLevelId = 1;
    [Header("使用Loading界面")]
    public bool IsLoading = false;

    BoxCollider2D _box;
    private void Start()
    {
        _box = GetComponent<BoxCollider2D>();
        _box.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null && character.RoleType == RoleType.Player)
        {
            if (IsLoading)
            {
                LoadSceneManager.Instance.LoadScene(TargetLevelId);
            }
            else
            {
                LoadSceneManager.Instance.LoadSceneAsync(TargetLevelId);
            }
        }
    }
}
