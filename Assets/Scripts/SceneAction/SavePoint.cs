using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SavePoint : MonoBehaviour
{
    BoxCollider2D _box;
    private void Start()
    {
        _box = GetComponent<BoxCollider2D>();
        _box.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null && character == GameConfig.Player)
        {
            PlayerData.Instance.SavePlayerInfo(PlayerData.Instance.CurPlayerInfo);
        }
    }
}
