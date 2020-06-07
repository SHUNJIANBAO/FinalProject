using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObj : MonoBehaviour
{
    public int KeyId;

    private void Start()
    {
        gameObject.SetActive(!LevelData.Instance.DoorIsOpen());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>()!=null)
        {
            if (LevelData.Instance.HasItem(KeyId))
            {
                LevelData.Instance.OpenDoor();
                gameObject.SetActive(false);
            }
            
        }
    }
}
