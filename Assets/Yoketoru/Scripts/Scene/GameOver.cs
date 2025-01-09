using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : OverlapScene
{
    enum State
    {
        None = -1,
        WaitStart,
        WaitInput,
        ToTitle,
        Retry,
    }
    SimpleState<State> state = new(State.None);

    private void Start()
    {
        state.SetNextState(State.WaitStart);
    }

    private void Update()
    {
        InitState();
        UpdateState();
    }

    void InitState()
    {
        if (!state.ChangeState())
        {
            return;
        }

        switch (state.CurrentState)
        {
            case State.ToTitle:
                gameInstance.RequestToTitle();
                break;

            case State.Retry:
                gameInstance.GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Start);
                Hided.AddListener(ToRetry);
                Hide();
                break;


        }
    }

    void UpdateState()
    {
        switch(state.CurrentState)
        {
            case State.WaitStart:
                if (canControl)
                {
                    state.SetNextState(State.WaitInput);
                }
                break;
            case State.WaitInput:
                UpdateInput();
                break;
        }
    }

    /// <summary>
    /// 入力待ちの更新
    /// </summary>
    void UpdateInput()
    {
        if (Input.GetButtonDown("ToTitle"))
        {
            state.SetNextState(State.ToTitle);
        }
        else if (Input.GetButtonDown("Submit"))
        {
            state.SetNextState(State.Retry);
        }
    }

    /// <summary>
    /// リトライを実行
    /// </summary>
    void ToRetry()
    {
        gameInstance.RequestRetry();
        SceneManager.UnloadSceneAsync("GameOver");
    }
}
