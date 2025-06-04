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
    /// ���� �� ��ε� �� �� �� �ܵ� �ε� (UI ����)
    /// </summary>
    public void LoadSceneAsync(string sceneName, Action onComplete = null)
    {
        Managers.Instance.Clear();
        Managers.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, onComplete));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action onComplete)
    {
        // �ε� UI ����
        Managers.UI.ShowLoadingUI("���� �ε��ϴ� ���Դϴ�. ��ø� ��ٷ��ּ���...");

        // �� �� �ε� (Single ��� = ���� �� �ڵ� ��ε� + ��ȯ)
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

        // �ε� UI ���� -> �ӽ� ����
        //Managers.UI.HideLoadingUI();

        onComplete?.Invoke();
    }


    /// <summary>
    /// ���� Additive ������� �߰� �ε�
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
    /// Additive�� �ε��� �� ����
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
            Debug.LogWarning($"[SceneManagerEx] '{sceneName}' ���� �ε�Ǿ� ���� �ʽ��ϴ�.");
            yield break;
        }

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
        while (!unloadOp.isDone)
            yield return null;

        onComplete?.Invoke();
    }
}
