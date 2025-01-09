using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ステージ全体を管理するスクリプト
/// </summary>
public class StageBehaviour : MonoBehaviour
{
    /// <summary>
    /// ゲームオーバーが要求されたときにInvokeするイベント。
    /// </summary>
    public UnityEvent GameOverRequested { get; } = new();

    /// <summary>
    /// コインを取ったときに、コインの基本点を受け取ってInvokeするイベント。
    /// </summary>
    public UnityEvent<int> GetCoinEmitted { get; } = new();

    List<IGameStateListener> gameStateListeners = new List<IGameStateListener>();

    void Start()
    {
        FindGameStateListeners();
    }

    /// <summary>
    /// ゲームスタートを通知する。
    /// </summary>
    public void CallGameStart()
    {
        for (int i = 0; i < gameStateListeners.Count; i++)
        {
            gameStateListeners[i].OnGameStart();
        }
    }

    /// <summary>
    /// ゲームオーバーを通知
    /// </summary>
    public void CallGameOver()
    {
        for (int i = 0; i < gameStateListeners.Count; i++)
        {
            gameStateListeners[i].OnGameOver();
        }
    }

    /// <summary>
    /// クリアを通知
    /// </summary>
    public void CallClear()
    {
        for (int i = 0; i < gameStateListeners.Count; i++)
        {
            gameStateListeners[i].OnClear();
        }
    }

    public void CallReset()
    {
        for (int i = 0; i < gameStateListeners.Count; i++)
        {
            gameStateListeners[i].OnReset();
        }
    }

    void FindGameStateListeners()
    {
        gameStateListeners.Clear();
        var rootObejcts = gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < rootObejcts.Length; i++)
        {
            var listeners = rootObejcts[i].GetComponentsInChildren<IGameStateListener>();
            for (int j=0; j<listeners.Length; j++)
            {
                listeners[j].GameStateListenerDestroyed.AddListener(RemoveGameStateListener);
                gameStateListeners.Add(listeners[j]);
            }
        }
    }

    /// <summary>
    /// ステージにある指定のインターフェースのインスタンスを返す。
    /// </summary>
    /// <typeparam name="T">指定するインターフェース</typeparam>
    /// <returns>インスタンス</returns>
    public List<T> GetStageInterfaces<T>()
    {
        var interfaces = new List<T>();

        var rootObejcts = gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < rootObejcts.Length; i++)
        {
            var listeners = rootObejcts[i].GetComponentsInChildren<T>();
            interfaces.AddRange(listeners);
        }

        return interfaces;
    }

    /// <summary>
    /// 指定のリスナーをリストから削除する。
    /// </summary>
    /// <param name="listener">削除したいリスナー</param>
    void RemoveGameStateListener(IGameStateListener listener)
    {
        listener.GameStateListenerDestroyed.RemoveListener(RemoveGameStateListener);
        gameStateListeners.Remove(listener);
    }
}
