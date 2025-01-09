using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class AllClear : SceneBehaviourBase
{
    [SerializeField]
    AudioSource bgmAudioSource = default;

    static float UncoverSeconds => 2f;
    static Color ToTitleColor => Color.white;
    static float ToTitleSeconds => 1f;
    enum State
    {
        None = -1,
        Show,
        WaitInput,
        ToTitle,
    }

    SimpleState<State> state = new(State.None);

    /// <summary>
    /// シーン開始
    /// </summary>
    /// <param name="gameSystem"></param>
    public override void StartScene(GameSystem gameSystem)
    {
        base.StartScene(gameSystem);

        // カバーを外す
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return GameSystem.Fade.Uncover(UncoverSeconds);
        state.SetNextState(State.Show);
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

        switch(state.CurrentState)
        {
            case State.Show:
                bgmAudioSource.Play();
                GetComponent<Animator>().SetBool("Show", true);
                break;

            case State.ToTitle:
                GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Click);
                StartCoroutine(ToTitle());
                break;
        }
    }

    void UpdateState()
    {
        switch(state.CurrentState)
        {
            case State.WaitInput:
                if (Input.GetButtonDown("Submit"))
                {
                    state.SetNextState(State.ToTitle);
                }
                break;
        }
    }

    /// <summary>
    /// ボタンをクリックした。
    /// </summary>
    public void OnClicked()
    {
        if (state.CurrentState == State.WaitInput)
        {
            state.SetNextState(State.ToTitle);
        }
    }

    /// <summary>
    /// 表示が完了したら、アニメから呼び出す。
    /// </summary>
    public void OnShowed()
    {
        state.SetNextState(State.WaitInput);
    }

    IEnumerator ToTitle()
    {
        string[] loadScenes =
        {
            "Title"
        };
        string[] unloadScenes =
        {
            "AllClear"
        };
        yield return GameSystem.Fade.Cover(ToTitleColor, ToTitleSeconds);

        GameSystem.SceneChanger.ChangeScene(loadScenes, unloadScenes);
    }
}
