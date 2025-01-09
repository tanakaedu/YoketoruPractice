using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// カウントダウンを制御
/// </summary>
public class CountDown : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent GameStarted = new();

    /// <summary>
    /// スケールの設定
    /// </summary>
    static float StartScale = 4f;
    static float JustScale = 1f;

    TextMeshProUGUI countDownText;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;

        countDownText = GetComponent<TextMeshProUGUI>();
        Color color = countDownText.color;
        color.a = 0;
        countDownText.color = color;
        countDownText.text = $"3";
    }

    /// <summary>
    /// カウントダウンを開始するときに呼び出す。
    /// </summary>
    public void StartCountDown(TinyAudio audio)
    {
        StartCoroutine(CountDownCoroutine(audio));
    }

    /// <summary>
    /// カウントダウンの進行
    /// </summary>
    /// <param name="tinyAudio">効果音</param>
    IEnumerator CountDownCoroutine(TinyAudio tinyAudio)
    {
        animator.enabled = false;

        for (int i=3;i>=1;i--)
        {
            yield return Animation($"{i}");
            tinyAudio.PlaySE(TinyAudio.SE.CountDown);
        }

        // Go
        yield return Animation("START!!");
        tinyAudio.PlaySE(TinyAudio.SE.StartPlay);
        GameStarted.Invoke();

        animator.enabled = true;
        animator.SetTrigger("Hide");
    }

    IEnumerator Animation(string text)
    {
        float time = 0;
        Color color = countDownText.color;
        while (time < 1f)
        {
            yield return null;
            countDownText.text = text;
            time += Time.deltaTime;
            color.a = time < 1f ? time : 1f;
            countDownText.color = color;
            countDownText.transform.localScale = Mathf.Lerp(StartScale, JustScale, color.a) * Vector3.one;
        }
        color.a = 1;
        countDownText.color = color;
        countDownText.transform.localScale = JustScale * Vector3.one;
    }
}
