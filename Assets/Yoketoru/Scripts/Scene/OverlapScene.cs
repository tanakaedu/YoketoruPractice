using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// クレジット、ゲームオーバー、クリアなどの、現在のシーンに重ね表示するシーンの共通クラス。
/// </summary>
public class OverlapScene : MonoBehaviour
{
    [SerializeField]
    protected TinyAudio tinyAudio = default;

    /// <summary>
    /// 表示が消えたら、Invokeするイベント。
    /// </summary>
    public UnityEvent Hided { get; private set; } = new();

    protected Animator animator;

    /// <summary>
    /// 操作可能状態
    /// </summary>
    protected bool canControl;

    protected Game gameInstance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ゲームのインスタンスを渡す。
    /// </summary>
    /// <param name="game">ゲーム操作インスタンス</param>
    public void SetGameInstance(Game game)
    {
        gameInstance = game;
    }

    /// <summary>
    /// 画面を表示。ゲームのインスタンスを受け取る。
    /// </summary>
    public void Show()
    {
        animator.SetBool("Show", true);
    }

    /// <summary>
    /// クレジット画面を消す。
    /// </summary>
    public void Hide()
    {
        canControl = false;
        animator.SetBool("Show", false);
    }

    /// <summary>
    /// 表示が完了したら、アニメから呼ぶ。
    /// </summary>
    public void OnShowed()
    {
        canControl = true;
    }

    /// <summary>
    /// アニメから、表示が消えた時に呼び出す
    /// </summary>
    public void OnHided()
    {
        Hided.Invoke();
    }

}
