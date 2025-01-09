using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// スコアの値オブジェクト
/// </summary>
public class Score
{
    public int Current { get; private set; } = 0;
    public UnityEvent<int> Changed { get; private set; } = new();

    int scoreMax;

    public Score(int max)
    {
        scoreMax = max;
    }

    /// <summary>
    /// 0点にする。
    /// </summary>
    public void Clear()
    {
        Current = 0;
        Changed.Invoke(Current);
    }

    /// <summary>
    /// 指定の得点を加算する。
    /// </summary>
    /// <param name="point"></param>
    public void Add(int point)
    {
        int newScore = Mathf.Clamp(Current + point, 0, scoreMax);
        if (newScore != Current)
        {
            Current = newScore;
            Changed.Invoke(Current);
        }
    }
}
