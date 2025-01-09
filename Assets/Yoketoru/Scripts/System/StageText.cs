using TMPro;
using UnityEngine;

public class StageText : MonoBehaviour
{
    /// <summary>
    /// ステージ数を設定する。
    /// </summary>
    /// <param name="stage">ステージ数。Stage1が1</param>
    public void Set(int stage)
    {
        GetComponent<TextMeshProUGUI>().text = $"Stage {stage}";
    }
}
