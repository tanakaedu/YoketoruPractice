using UnityEngine;

/// <summary>
/// コインの数を管理するクラス。
/// </summary>
public class CoinCounter
{
    int count;

    /// <summary>
    /// タグのオブジェクトを数える。
    /// </summary>
    public void CountCoin()
    {
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        count = coins.Length;
    }

    /// <summary>
    /// アイテムを1つ減らす。
    /// </summary>
    /// <returns>全部取り切っていたら、trueを返す。</returns>
    public bool Decrement()
    {
        count--;
        return (count <= 0);
    }
}