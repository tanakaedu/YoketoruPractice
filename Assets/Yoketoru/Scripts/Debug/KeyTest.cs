using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTest : MonoBehaviour
{
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!Mathf.Approximately(h, 0) || !Mathf.Approximately(v, 0))
        {
            Debug.Log($"{h}, {v}");
        }
    }
}
