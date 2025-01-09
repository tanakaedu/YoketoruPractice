using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Animator animator;
    Image image;
    bool isPlaying;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        image = GetComponentInChildren<Image>();
    }

    /// <summary>
    /// 画面を隠す処理のコルーチン。
    /// </summary>
    /// <param name="color">色</param>
    /// <param name="time">隠す秒数。0のときは即時</param>
    public IEnumerator Cover(Color color, float time = 0)
    {
        image.color = color;
        if (Mathf.Approximately(time, 0))
        {
            animator.SetBool("Cover", true);
            animator.SetBool("Force", true);
            isPlaying = false;
        }
        else
        {
            // アニメ
            animator.SetBool("Cover", true);
            animator.SetBool("Force", false);
            animator.SetFloat("Speed", 1f / time);
            isPlaying = true;
            while (isPlaying)
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// 画面を表示。
    /// </summary>
    /// <param name="time">演出秒数。0や省略で即時</param>
    public IEnumerator Uncover(float time = 0)
    {
        if (Mathf.Approximately(time, 0))
        {
            animator.SetBool("Cover", false);
            animator.SetBool("Force", true);
            isPlaying = false;
        }
        else
        {
            // アニメ
            animator.SetBool("Cover", false);
            animator.SetBool("Force", false);
            animator.SetFloat("Speed", 1f / time);
            isPlaying = true;
            while (isPlaying)
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// アニメが終了したら、これを呼び出す。
    /// </summary>
    public void Animated()
    {
        isPlaying = false;
    }

}
