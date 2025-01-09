using UnityEngine.Events;

/// <summary>
/// ゲームオーバーを通知するオブジェクトに実装するインターフェース。
/// </summary>
public interface IGameOverEmitter
{
    /// <summary>
    /// ゲームオーバーを要求するメソッドを登録するイベント。
    /// </summary>
    UnityEvent GameOverRequest { get; }
}
