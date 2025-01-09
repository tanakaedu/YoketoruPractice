using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : SceneBehaviourBase
{
    static float UncoverSeconds => 1;
    static string CreditsSceneName => "Credits";

    enum State
    {
        None = -1,
        /// <summary>
        /// 操作受付中
        /// </summary>
        CanControl,
        /// <summary>
        /// クレジット画面表示中
        /// </summary>
        Credits,
        /// <summary>
        /// ゲーム開始
        /// </summary>
        GameStart,
    }

    SimpleState<State> state = new(State.None);
    Credits credits;

    public override void StartScene(GameSystem gameSystem)
    {
        base.StartScene(gameSystem);

        StartCoroutine(StartTitle());
    }

    IEnumerator StartTitle()
    {
        // スコアを設定
        var scoreText = FindObjectOfType<ScoreText>();
        if (scoreText != null)
        {
            scoreText.OnChanged(GameSystem.Score.Current);
        }

        yield return GameSystem.Fade.Uncover(UncoverSeconds);

        // Start Title Control
        state.SetNextState(State.CanControl);
    }

    private void Update()
    {
        InitState();
        UpdateState();
    }

    private void OnDestroy()
    {
        if (credits != null)
        {
            credits.Hided.RemoveAllListeners();
        }
    }

    public void OnStartClicked()
    {
        if (state.CurrentState == State.CanControl)
        {
            state.SetNextState(State.GameStart);
        }
    }

    /// <summary>
    /// クレジットボタンが押された
    /// </summary>
    public void OnCreditsClicked()
    {
        if (state.CurrentState == State.CanControl)
        {
            state.SetNextState(State.Credits);
        }
    }

    void InitState()
    {
        if (!state.ChangeState())
        {
            return;
        }

        switch (state.CurrentState)
        {
            case State.Credits:
                StartCoroutine(InitCredits());
                break;

            case State.GameStart:
                StartCoroutine(GameStart());
                break;
        }
    }

    void UpdateState()
    {
        switch (state.CurrentState)
        {
            case State.CanControl:
                UpdateControl();
                break;
        }
    }

    /// <summary>
    /// クレジットシーンの表示開始
    /// </summary>
    IEnumerator InitCredits()
    {
        if (credits == null)
        {
            yield return SceneManager.LoadSceneAsync(CreditsSceneName, LoadSceneMode.Additive);

            // シーン上のゲームオブジェクトの初期化のため、1フレーム待つ
            yield return null;

            credits = GameObject.FindObjectOfType<Credits>();
            credits.Hided.AddListener(OnCreditsHided);
        }

        GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Click);
        credits.Show();
    }

    /// <summary>
    /// クレジットが閉じたら、タイトル画面の操作を再開。
    /// </summary>
    void OnCreditsHided()
    {
        state.SetNextStateForce(State.CanControl);
    }

    void UpdateControl()
    {
        if (Input.GetButtonDown("Submit"))
        {
            state.SetNextState(State.GameStart);
        }
        else if (Input.GetButtonDown("Credits"))
        {
            state.SetNextState(State.Credits);
        }
    }

    static float CoverSeconds => 1f;
    static Color CoverColor => Color.black;

    /// <summary>
    /// ゲームシーンに切り替える処理。
    /// </summary>
    IEnumerator GameStart()
    {
        GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Start);
        GameSystem.NewGame();

        yield return GameSystem.Fade.Cover(CoverColor, CoverSeconds);

        string[] loadScenes =
        {
            "Game",
            GameSystem.Stage.StageSceneName,
        };
        string[] unloadScenes =
        {
            "Title",
            "Credits",
        };
        GameSystem.SceneChanger.ChangeScene(loadScenes, unloadScenes);
    }
}
