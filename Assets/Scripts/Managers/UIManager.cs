using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase
{
    private readonly Dictionary<string, UIBase> _uiDic = new();

    private Transform _uiRoot;

    private string _loadingUIPath => TypeToPath.GetPath(Enums.UIType.Loading);
    private LoadingUI _loadingUI;

    public override void Init()
    {
        base.Init();

        if (_uiRoot == null)
        {
            var go = new GameObject { name = "@UI_Root" };
            Object.DontDestroyOnLoad(go);
            _uiRoot = go.transform;
        }
    }

    public override void Clear()
    {
        foreach (var ui in _uiDic.Values)
        {
            if (ui != null)
                Object.Destroy(ui.gameObject);
        }
        _uiDic.Clear();
        _loadingUI = null;
    }

    public T ShowUI<T>(Enums.UIType type) where T : UIBase
    {
        string prefabPath = TypeToPath.GetPath(type);

        if (_uiDic.TryGetValue(prefabPath, out var existingUI))
        {
            existingUI.gameObject.SetActive(true);
            return existingUI as T;
        }

        var prefab = Managers.Resource.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogWarning($"{prefabPath} 경로의 UI는 존재하지 않음");
            return null;
        }

        var go = Object.Instantiate(prefab, _uiRoot);
        go.name = prefab.name;

        var ui = go.GetComponent<T>();
        if (ui == null)
        {
            Debug.LogWarning($"[UIManager] UI prefab '{prefabPath}'에 {typeof(T)} 컴포넌트가 없습니다.");
            Object.Destroy(go);
            return null;
        }

        ui.SetComponent();
        ui.Init();

        _uiDic.Add(prefabPath, ui);

        return ui;
    }

    public void HideUI(string prefabPath)
    {
        if (_uiDic.TryGetValue(prefabPath, out var ui))
        {
            ui.gameObject.SetActive(false);
        }
    }

    // --------------------------
    // ▼ Loading UI 전용 기능 ▼
    // --------------------------

    public void ShowLoadingUI(string tooltipText)
    {
        if (_loadingUI == null)
        {
            _loadingUI = ShowUI<LoadingUI>(Enums.UIType.Loading);
            if (_loadingUI == null)
            {
                Debug.LogError("LoadingUI 생성 실패");
                return;
            }

            // 리소스 로고 로드 (Resources 폴더 기준)
            var logoSprite = Resources.Load<Sprite>("UI/Logo");
            _loadingUI.SetLoadingImage(logoSprite);
        }

        _loadingUI.gameObject.SetActive(true);
        _loadingUI.SetTooltip(tooltipText);
        _loadingUI.SetProgress(0f);
    }

    public void UpdateLoadingProgress(float progress)
    {
        _loadingUI?.SetProgress(progress);
    }

    public void HideLoadingUI()
    {
        if (_loadingUI != null)
            _loadingUI.gameObject.SetActive(false);
    }
}
