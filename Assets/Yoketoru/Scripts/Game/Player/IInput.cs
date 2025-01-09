using UnityEngine;

public interface IInput
{
    /// <summary>
    /// 前回から記録した移動量を読み取って、クリアする。
    /// </summary>
    /// <returns>長さ0-1の範囲のベクトル</returns>
    Vector2 GetValue();

    /// <summary>
    /// Updateから呼び出して、入力の値を更新する。
    /// </summary>
    void Update();
}
