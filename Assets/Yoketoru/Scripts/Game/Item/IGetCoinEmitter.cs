using UnityEngine.Events;

/// <summary>
/// コインを取った報告をするインターフェース。
/// </summary>
public interface IGetCoinEmitter
{
    /// <summary>
    /// コインを取ったら、コインの基本点を引数に渡して呼び出すイベントを登録する。
    /// </summary>
    UnityEvent<int> CoinGot { get; }
}
