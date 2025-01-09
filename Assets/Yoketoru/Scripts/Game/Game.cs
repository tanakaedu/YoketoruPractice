#define DEBUG_KEY

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : SceneBehaviourBase
{
    /// <summary>
    /// 開始秒数
    /// </summary>
    static float StartGameTime => 99.99f;

    [SerializeField]
    AudioSource bgmAudioSource = default;
    [SerializeField]
    CountDown countDown = default;

    static float StartUncoverSeconds => 1f;

    // タイトルへ
    static float ToTitleSeconds => 1f;
    static Color ToTitleColor => Color.black;

    // 次のステージへ
    static Color NextStageColor => Color.white;
    static float NextStageSeconds => 1.5f;

    // 全ステージクリアへ
    static Color AllClearColor => Color.white;
    static float AllClearSeconds => 2f;

    enum State
    {
        None = -1,
        CountDown,
        Play,
        GameOver,
        Clear,
        Retry,
        ToTitle,
        NextStage,
        SceneDone,
    }

    SimpleState<State> state = new(State.None);
    Player player;
    Player PlayerInstance
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }
            return player;
        }
    }
    StageBehaviour stageBehaviour;

    ScoreText scoreText;
    StageText stageText;
    TimeText timeText;

    CoinCounter itemCounter = new();

    public override void StartScene(GameSystem gameSystem)
    {
        base.StartScene(gameSystem);

        // スコアの設定
        scoreText = FindObjectOfType<ScoreText>();
        if (scoreText != null)
        {
            scoreText.OnChanged(gameSystem.Score.Current);
            gameSystem.Score.Changed.AddListener(scoreText.OnChanged);
        }

        // ステージの設定
        stageText = FindObjectOfType<StageText>();
        if (stageText != null)
        {
            stageText.Set(gameSystem.Stage.Current);
        }

        // タイムの設定
        timeText = FindObjectOfType<TimeText>();
        if (timeText != null)
        {
            gameSystem.GameTime.Changed.AddListener(timeText.OnChanged);
            gameSystem.GameTime.Set(StartGameTime);
        }

        // アイテムの数を数える
        itemCounter.CountCoin();

        // コインを取った処理を登録
        stageBehaviour = FindObjectOfType<StageBehaviour>();
        var coinEmitters = stageBehaviour.GetStageInterfaces<IGetCoinEmitter>();
        foreach(var coinEmitter in coinEmitters)
        {
            coinEmitter.CoinGot.AddListener(GotCoin);
        }

        // ゲームオーバーの報告を登録
        var gameOverEmitters = stageBehaviour.GetStageInterfaces<IGameOverEmitter>();
        foreach(var gameOverEmitter in gameOverEmitters)
        {
            gameOverEmitter.GameOverRequest.AddListener(RequestGameOver);
        }

        StartCoroutine(GameStartCoroutine());
    }



    private void OnDestroy()
    {
        if (scoreText != null)
        {
            GameSystem.Score.Changed.RemoveListener(scoreText.OnChanged);
        }
        if (timeText != null)
        {
            GameSystem.GameTime.Changed.RemoveListener(timeText.OnChanged);
        }
    }

    IEnumerator GameStartCoroutine()
    {
        yield return GameSystem.Fade.Uncover(StartUncoverSeconds);
        state.SetNextStateForce(State.CountDown);
    }

    private void Update()
    {
        UpdateState();
        HideMouseCursor();
    }

    private void FixedUpdate()
    {
        InitState();
        FixedUpdateState();
    }

    void HideMouseCursor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = true;
        }
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void InitState()
    {
        if (!state.ChangeState())
        {
            return;
        }

        switch (state.CurrentState)
        {
            case State.CountDown:
                countDown.StartCountDown(GameSystem.TinyAudio);
                countDown.GameStarted.AddListener(() => state.SetNextState(State.Play));
                break;

            case State.Play:
                countDown.GameStarted.RemoveAllListeners();
                bgmAudioSource.Play();
                stageBehaviour.CallGameStart();
                break;

            case State.GameOver:
                bgmAudioSource.Stop();
                GameSystem.TinyAudio.PlaySE(TinyAudio.SE.GameOver);
                stageBehaviour.CallGameOver();
                StartCoroutine(ShowOverlapScene("GameOver"));
                break;

            case State.Clear:
                bgmAudioSource.Stop();
                GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Clear);
                stageBehaviour.CallClear();
                StartCoroutine(ShowOverlapScene("Clear"));
                break;

            case State.Retry:
                GameSystem.Score.Clear();
                GameSystem.GameTime.Set(StartGameTime);
                stageBehaviour.CallReset();
                state.SetNextState(State.CountDown);
                break;

            case State.ToTitle:
                GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Cancel);
                StartCoroutine(ToTitleCoroutine());
                break;

            case State.NextStage:
                GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Click);
                string unloadStage = GameSystem.Stage.StageSceneName;
                if (GameSystem.Stage.Next())
                {
                    // エンディングへ
                    StartCoroutine(ToAllClear(unloadStage));
                }
                else
                {
                    // 次のステージへ
                    StartCoroutine(NextStage(unloadStage));
                }
                break;
        }
    }

    /// <summary>
    /// 全ステージクリアへ
    /// </summary>
    /// <returns></returns>
    IEnumerator ToAllClear(string unloadStage)
    {
        string[] loadScenes =
        {
            "AllClear",
        };
        string[] unloadScenes =
        {
            "Game",
            unloadStage,
            "Clear",
        };

        yield return GameSystem.Fade.Cover(AllClearColor, AllClearSeconds);
        GameSystem.SceneChanger.ChangeScene(loadScenes, unloadScenes);
    }

    /// <summary>
    /// 次のステージへ
    /// </summary>
    /// <returns></returns>
    IEnumerator NextStage(string unloadStage)
    {
        string[] loadScenes =
        {
            "Game",
            GameSystem.Stage.StageSceneName,
        };
        string[] unloadScenes =
        {
            "Game",
            unloadStage,
            "Clear",
        };

        yield return GameSystem.Fade.Cover(NextStageColor, NextStageSeconds);
        GameSystem.SceneChanger.ChangeScene(loadScenes, unloadScenes);
    }

    /// <summary>
    /// タイトルへ切り替える。
    /// </summary>
    /// <returns></returns>
    IEnumerator ToTitleCoroutine()
    {
        string[] loadScenes =
        {
            "Title"
        };
        string[] unloadScenes =
        {
            "Game",
            GameSystem.Stage.StageSceneName,
            "GameOver",
        };
        yield return GameSystem.Fade.Cover(ToTitleColor, ToTitleSeconds);
        GameSystem.SceneChanger.ChangeScene(loadScenes, unloadScenes);
    }

    /// <summary>
    /// タイトル切り替えを要求する。
    /// </summary>
    public void RequestToTitle()
    {
        state.SetNextState(State.ToTitle);
    }

    /// <summary>
    /// リトライを要求する。
    /// </summary>
    public void RequestRetry()
    {
        state.SetNextState(State.Retry);
    }

    /// <summary>
    /// 次のステージへの切り替えを要求する。
    /// </summary>
    public void RequestNextStage()
    {
        state.SetNextState(State.NextStage);
    }

    /// <summary>
    /// ゲームオーバーを要求
    /// </summary>
    void RequestGameOver()
    {
        state.SetNextState(State.GameOver);
    }

    /// <summary>
    /// クリアを要求。
    /// </summary>
    void RequestClear()
    {
        state.SetNextStateForce(State.Clear);
    }

    /// <summary>
    /// 得点アイテムを取ったら、基準点を渡して呼び出す。
    /// アイテムの数を減らして、0になったらクリアを呼び出す。
    /// </summary>
    /// <param name="point">基準点</param>
    /// <returns>クリア時、trueを返す。</returns>
    void GotCoin(int point)
    {
        GameSystem.TinyAudio.PlaySE(TinyAudio.SE.Item);
        GameSystem.Score.Add(Mathf.FloorToInt(point * GameSystem.GameTime.Current));
        if (itemCounter.Decrement())
        {
            RequestClear();
        }
    }

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    IEnumerator ShowOverlapScene(string sceneName)
    {
        var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return async;
        yield return null;

        var overlap = FindObjectOfType<OverlapScene>();
        overlap.SetGameInstance(this);
        overlap.Show();
    }

    void UpdateState()
    {
        switch (state.CurrentState)
        {
            case State.Play:
                UpdateDebugKey();
                break;
        }
    }

    void FixedUpdateState()
    {
        switch (state.CurrentState)
        {
            case State.Play:
                GameSystem.GameTime.Update(Time.deltaTime);
                break;
        }
    }

    [System.Diagnostics.Conditional("DEBUG_KEY")]
    void UpdateDebugKey()
    {
        if (Input.GetButtonDown("DebugGameOver"))
        {
            RequestGameOver();
        }
        else if (Input.GetButtonDown("DebugClear"))
        {
            RequestClear();
        }
        else if (Input.GetButtonDown("Submit"))
        {
            GameSystem.Score.Add(100);
        }
    }
}
