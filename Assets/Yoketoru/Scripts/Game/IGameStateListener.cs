using UnityEngine.Events;

/// <summary>
/// ゲームの状態変化を受け取るリスナー。
/// </summary>
public interface IGameStateListener
{
    /// <summary>
    /// このゲームオブジェクトをDestoryするときにInvokeするイベント。
    /// 呼び出し配列から削除するメソッドを登録する。
    /// </summary>
    UnityEvent<IGameStateListener> GameStateListenerDestroyed { get; }

    /// <summary>
    /// ゲームの開始状態に戻すときに呼び出される。
    /// </summary>
    void OnReset();

    /// <summary>
    /// ゲームが開始されたときに呼び出される。
    /// </summary>
    void OnGameStart();

    /// <summary>
    /// ゲームオーバーになったときに呼び出される。
    /// </summary>
    void OnGameOver();

    /// <summary>
    /// クリアしたときに呼び出される。
    /// </summary>
    void OnClear();
}
