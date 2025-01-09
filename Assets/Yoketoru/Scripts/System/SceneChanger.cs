using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを切り替える機能を提供するクラス。
/// </summary>
public class SceneChanger : MonoBehaviour
{
    /// <summary>
    /// シーンの切り替えが完了したら、Invokeするイベント。
    /// </summary>
    public UnityEvent Changed { get; private set; } = new UnityEvent();

    /// <summary>
    /// 読み込みたいシーン名と、解放したいシーン名を受け取って、
    /// 処理順は、アンロードが先で、ロードがあと。
    /// 非同期で処理を開始する。
    /// すべての処理が完了したら、ChangedイベントをInvoke。
    /// </summary>
    /// <param name="loadScenes">読み込みたいシーン名</param>
    /// <param name="unloadScenes">解放したいシーン名</param>
    public void ChangeScene(string[] loadScenes, string[] unloadScenes)
    {
        // 読み込み
        StartCoroutine(ChangeSceneCoroutine(loadScenes, unloadScenes));
    }

    IEnumerator ChangeSceneCoroutine(string[] loadScenes, string[] unloadScenes)
    {
        List<AsyncOperation> operations = new List<AsyncOperation>();

        // 解放
        for (int i = 0; i < unloadScenes.Length; i++)
        {
            if (IsSceneLoaded(unloadScenes[i]))
            {
                operations.Add(SceneManager.UnloadSceneAsync(unloadScenes[i]));
            }
        }
        yield return WaitChanged(operations);

        // 読み込み
        operations.Clear();
        for (int i = 0; i < loadScenes.Length; i++)
        {
            if (!IsSceneLoaded(loadScenes[i]))
            {
                operations.Add(SceneManager.LoadSceneAsync(loadScenes[i], LoadSceneMode.Additive));
            }
        }
        yield return WaitChanged(operations);

        // 切り替え完了
        Changed.Invoke();
    }

    /// <summary>
    /// 引数の非同期処理が、すべて完了するのを待つ。
    /// </summary>
    /// <param name="operations"></param>
    /// <returns></returns>
    IEnumerator WaitChanged(List<AsyncOperation> operations)
    {
        for (int i = 0; i < operations.Count; i++)
        {
            yield return operations[i];
        }
    }

    /// <summary>
    /// 指定のファイルが読み込まれているかを確認。
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <returns>シーンが読み込まれていたら、true</returns>
    bool IsSceneLoaded(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (scene == null)
        {
            return false;
        }

        return scene.isLoaded;
    }
}
