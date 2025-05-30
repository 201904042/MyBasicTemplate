using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : ManagerBase
{
    private GameObject loadingUIInstance;
    private LoadingUI loadingUI;

    public override void Init()
    {
        base.Init();
        // �ʱ�ȭ �ʿ�� ����
    }

    public override void Clear()
    {
        base.Clear();
        // �ʱ�ȭ �ʿ�� ����
    }

    // �� �ε� (���� �� ��ε� ����)
    public void LoadSceneAsync(string sceneName, Action onComplete = null)
    {
        Managers.Instance.Clear(); // ���� �Ŵ��� Ŭ����(�ʿ��)

        // �ڷ�ƾ���� ó�� (Managers�� �ڷ�ƾ ������ �ִٰ� ����)
        Managers.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, onComplete));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action onComplete)
    {
        // ���� �� ��ε�
        var currentScene = SceneManager.GetActiveScene();
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        while (!unloadOp.isDone)
        {
            yield return null;
        }

        // �ε� UI ǥ��
        ShowLoadingUI();

        // �� �� �ε�
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;
        while (loadOp.progress < 0.9f)
        {
            UpdateLoadingProgress(loadOp.progress);
            yield return null;
        }

        // �ε� �Ϸ� �������� ����� 0.9
        UpdateLoadingProgress(1f);

        // �� Ȱ��ȭ
        loadOp.allowSceneActivation = true;

        // �� Ȱ��ȭ �Ϸ���� ���
        while (!loadOp.isDone)
        {
            yield return null;
        }

        // �ε� UI ����
        HideLoadingUI();

        // �Ϸ� �ݹ�
        onComplete?.Invoke();
    }

    private void ShowLoadingUI()
    {
        if (loadingUIInstance == null)
        {
            loadingUIInstance = Managers.Resource.Instantiate(TypeToPath.GetPath(UIType.Loading));
            loadingUI = loadingUIInstance.GetComponent<LoadingUI>();

            // �ε� �̹��� ���� (Resources ������ �ΰ� �̹����� �ִٰ� ����) => ���� ����
            var logoSprite = Resources.Load<Sprite>("UI/Logo");
            loadingUI.SetLoadingImage(logoSprite);

            // ���� �ؽ�Ʈ ����
            loadingUI.SetTooltip("���� �ε��ϴ� ���Դϴ�. ��ø� ��ٷ��ּ���...");
        }
        loadingUIInstance.SetActive(true);
        UpdateLoadingProgress(0f);
    }


    private void UpdateLoadingProgress(float progress)
    {
        loadingUI?.SetProgress(progress);
    }

    private void HideLoadingUI()
    {
        if (loadingUIInstance != null)
            loadingUIInstance.SetActive(false);
    }
}
