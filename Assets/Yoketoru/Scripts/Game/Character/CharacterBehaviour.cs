using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 爆弾やアイテムなどの状態を総合的に管理するクラス。
/// </summary>
public class CharacterBehaviour : MonoBehaviour, IGameStateListener
{
    enum State
    {
        None = -1,
        Play,
        End,
    }

    public UnityEvent<IGameStateListener> GameStateListenerDestroyed { get; private set; } = new();

    /// <summary>
    /// プレイ状態ならtrueを返す。
    /// </summary>
    public bool IsPlaying => state.CurrentState == State.Play;

    SimpleState<State> state = new(State.None);
    IStartStop[] startStops;

    void Awake()
    {
        startStops = GetComponents<IStartStop>();
    }

    void FixedUpdate()
    {
        InitState();
    }

    void OnDestroy()
    {
        GameStateListenerDestroyed.Invoke(this);
    }

    void InitState()
    {
        if (!state.ChangeState())
        {
            return;
        }

        switch(state.CurrentState)
        {
            case State.Play:
                for (int i=0;i<startStops.Length;i++)
                {
                    startStops[i].OnGameStarted();
                }
                break;

            case State.End:
                for (int i = 0; i < startStops.Length; i++)
                {
                    startStops[i].OnGameStopped();
                }
                break;
        }
    }

    public void OnClear()
    {
        state.SetNextState(State.End);
    }

    public void OnGameOver()
    {
        state.SetNextState(State.End);
    }

    public void OnGameStart()
    {
        state.SetNextState(State.Play);
    }

    public void OnReset()
    {
        state.SetNextState(State.End);
    }
}
