using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTime
{
    /// <summary>
    /// 現在の残り秒数
    /// </summary>
    public float Current;

    public UnityEvent<int> Changed { get; private set; } = new();

    /// <summary>
    /// 前回表示した秒数を100倍した値。不一致なら、表示呼び出し。
    /// </summary>
    int lastTime100;

    /// <summary>
    /// 新しい値を設定する。
    /// </summary>
    /// <param name="value">新規の値</param>
    public void Set(float value)
    {
        Current = value;
        lastTime100 = Mathf.FloorToInt(Current * 100f);
        Changed.Invoke(lastTime100);
    }

    /// <summary>
    /// 指定秒数経過
    /// </summary>
    /// <param name="delta"></param>
    public void Update(float delta)
    {
        Current = Mathf.Max(Current - delta, 0f);
        int newTime = Mathf.FloorToInt(Current * 100f);
        if (newTime != lastTime100)
        {
            Changed.Invoke(newTime);
            lastTime100 = newTime;
        }
    }
}
