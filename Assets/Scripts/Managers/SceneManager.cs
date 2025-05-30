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
        // 초기화 필요시 구현
    }

    public override void Clear()
    {
        base.Clear();
        // 초기화 필요시 구현
    }

    // 씬 로드 (기존 씬 언로드 포함)
    public void LoadSceneAsync(string sceneName, Action onComplete = null)
    {
        Managers.Instance.Clear(); // 기존 매니저 클리어(필요시)

        // 코루틴으로 처리 (Managers에 코루틴 실행기능 있다고 가정)
        Managers.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, onComplete));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action onComplete)
    {
        // 기존 씬 언로드
        var currentScene = SceneManager.GetActiveScene();
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        while (!unloadOp.isDone)
        {
            yield return null;
        }

        // 로딩 UI 표시
        ShowLoadingUI();

        // 새 씬 로드
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;
        while (loadOp.progress < 0.9f)
        {
            UpdateLoadingProgress(loadOp.progress);
            yield return null;
        }

        // 로드 완료 직전까지 진행률 0.9
        UpdateLoadingProgress(1f);

        // 씬 활성화
        loadOp.allowSceneActivation = true;

        // 씬 활성화 완료까지 대기
        while (!loadOp.isDone)
        {
            yield return null;
        }

        // 로딩 UI 종료
        HideLoadingUI();

        // 완료 콜백
        onComplete?.Invoke();
    }

    private void ShowLoadingUI()
    {
        if (loadingUIInstance == null)
        {
            loadingUIInstance = Managers.Resource.Instantiate(TypeToPath.GetPath(UIType.Loading));
            loadingUI = loadingUIInstance.GetComponent<LoadingUI>();

            // 로딩 이미지 설정 (Resources 폴더에 로고 이미지가 있다고 가정) => 추후 수정
            var logoSprite = Resources.Load<Sprite>("UI/Logo");
            loadingUI.SetLoadingImage(logoSprite);

            // 툴팁 텍스트 설정
            loadingUI.SetTooltip("씬을 로드하는 중입니다. 잠시만 기다려주세요...");
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
