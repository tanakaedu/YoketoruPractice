/// <summary>
/// ステージ数を表す値オブジェクト
/// 1=Stage1。0はタイトルなど。
/// </summary>
public class Stage
{
    /// <summary>
    /// 最終ステージ
    /// </summary>
    public static int LastStage => 2;

    /// <summary>
    /// 現在のステージのシーン名を返す。
    /// </summary>
    public string StageSceneName => $"Stage{Current}";

    /// <summary>
    /// 現在のステージ数
    /// </summary>
    public int Current { get; private set; }

    /// <summary>
    /// 最初のステージにする。
    /// </summary>
    public void Start(int start = 1)
    {
        Current = start;
    }

    /// <summary>
    /// 次のステージに更新。ステージクリアしたらfalse。最終ステージをクリアしていたらtrueを返す。
    /// </summary>
    /// <returns>最終ステージをクリアしていたら、ステージ数はそのままで、trueを返す。</returns>
    public bool Next()
    {
        if (Current == LastStage)
        {
            return true;
        }

        Current++;
        return false;
    }
}
