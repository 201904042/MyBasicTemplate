using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : ManagerBase
{
    public override void Init()
    {
        base.Init();
    }

    public override void Clear()
    {
        base.Clear();
    }

    /// <summary>
    /// 기존 씬 언로드 후 새 씬 단독 로드 (UI 포함)
    /// </summary>
    public void LoadSceneAsync(string sceneName, Action onComplete = null)
    {
        Managers.Instance.Clear();
        Managers.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, onComplete));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action onComplete)
    {
        // 로딩 UI 시작
        Managers.UI.ShowLoadingUI("씬을 로드하는 중입니다. 잠시만 기다려주세요...");

        // 새 씬 로드 (Single 모드 = 기존 씬 자동 언로드 + 전환)
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
        {
            Managers.UI.UpdateLoadingProgress(loadOp.progress);
            yield return null;
        }

        Managers.UI.UpdateLoadingProgress(1f);
        loadOp.allowSceneActivation = true;

        while (!loadOp.isDone)
            yield return null;

        // 로딩 UI 종료 -> 임시 제거
        //Managers.UI.HideLoadingUI();

        onComplete?.Invoke();
    }


    /// <summary>
    /// 씬을 Additive 방식으로 추가 로드
    /// </summary>
    public void AddSceneAsync(string sceneName, Action onComplete = null)
    {
        Managers.Instance.StartCoroutine(AddSceneCoroutine(sceneName, onComplete));
    }

    private IEnumerator AddSceneCoroutine(string sceneName, Action onComplete)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        onComplete?.Invoke();
    }

    /// <summary>
    /// Additive로 로드한 씬 제거
    /// </summary>
    public void UnloadSceneAsync(string sceneName, Action onComplete = null)
    {
        Managers.Instance.StartCoroutine(UnloadSceneCoroutine(sceneName, onComplete));
    }

    private IEnumerator UnloadSceneCoroutine(string sceneName, Action onComplete)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded)
        {
            Debug.LogWarning($"[SceneManagerEx] '{sceneName}' 씬이 로드되어 있지 않습니다.");
            yield break;
        }

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
        while (!unloadOp.isDone)
            yield return null;

        onComplete?.Invoke();
    }
}
