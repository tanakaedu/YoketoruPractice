using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField, Tooltip("アニメを再生する秒数")]
    float seconds = 1f;
    [SerializeField]
    Texture[] textures = default;

    Material material;
    int index;
    float time;

    private void Awake()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
        index = 0;
        material.SetTexture("_MainTex", textures[index]);
    }

    private void Start()
    {
        index = 0;
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        int newIndex = Mathf.FloorToInt(time * textures.Length / seconds);
        if (newIndex >= textures.Length)
        {
            // アニメ終了
            Destroy(gameObject);
            return;
        }

        // アニメ設定
        if (newIndex != index)
        {
            index = newIndex;
            material.SetTexture("_MainTex", textures[index]);
        }
    }
}
