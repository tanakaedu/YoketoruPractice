using TMPro;
using UnityEngine;

/// <summary>
/// スコア表示を管理する
/// </summary>
public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// スコアが変わった時に呼び出すメソッド。
    /// </summary>
    /// <param name="score">新しいスコア</param>
    public void OnChanged(int score)
    {
        scoreText.text = $"{score:d06}";
    }
}
