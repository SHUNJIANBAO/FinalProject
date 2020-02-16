using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //UIManager.Instance.OpenWindow<TestWindow>();

        }
        if (Input.GetKeyDown(KeyCode.K))
        {
           // UIManager.Instance.CloseWindow<TestWindow>();

        }
    }
}
