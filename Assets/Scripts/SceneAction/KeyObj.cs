using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObj : MonoBehaviour
{
    public int KeyId;
    public float RotateSpeed;
    // Use this for initialization
    void Start () {
        gameObject.SetActive(!LevelData.Instance.HasItem(KeyId));

    }

    private void Update()
    {
        transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            LevelData.Instance.GetItem(KeyId);
            gameObject.SetActive(false);
        }

    }
}
