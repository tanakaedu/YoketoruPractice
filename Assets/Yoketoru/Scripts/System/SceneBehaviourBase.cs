using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンの管理用スクリプトの基底クラス。
/// GameSystemのインスタンスが必要なクラスは、これを継承。
/// </summary>
public class SceneBehaviourBase : MonoBehaviour
{
    public GameSystem GameSystem { get; private set; }

    /// <summary>
    /// シーンを開始するメソッド。
    /// これをオーバーライドして、各シーンを開始する。
    /// </summary>
    /// <param name="gameSystem">ゲームシステムのインスタンス</param>
    public virtual void StartScene(GameSystem gameSystem)
    {
        GameSystem = gameSystem;
    }
}
