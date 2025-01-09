using UnityEngine;

public interface IMover
{
    /// <summary>
    /// 0-1の範囲で移動方向のベクトルを受け取って、移動させる。
    /// </summary>
    /// <param name="move">長さ0-1の範囲の移動方向ベクトル</param>
    void Move(Vector2 move);
}
