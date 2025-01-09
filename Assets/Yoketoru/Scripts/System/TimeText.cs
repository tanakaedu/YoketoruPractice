using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    TextMeshProUGUI timeText;

    void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 時間が変わったら呼び出して、テキストへ反映
    /// </summary>
    /// <param name="time100">新しい秒数の100倍</param>
    public void OnChanged(int time100)
    {
        int milli = time100 % 100;
        int sec = time100 / 100;
        timeText.text = $"{sec}<size=48>.{milli:d02}</size>";
    }
}
