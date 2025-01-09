/// <summary>
/// ゲームが開始したり、停止したときに呼び出されるメソッドを定義するインターフェース。
/// </summary>
public interface IStartStop
{
    /// <summary>
    /// ゲームが開始したときに呼び出されるメソッド。
    /// </summary>
    public void OnGameStarted();

    /// <summary>
    /// ゲームが停止したときに呼び出されるメソッド。
    /// </summary>
    public void OnGameStopped();
}
