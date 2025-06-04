using UnityEngine;
using UnityEngine.UI;

public class SceneTest : MonoBehaviour
{
    [Header("�׽�Ʈ�� �� ����")]
    public Enums.Scene targetScene;

    private void Awake()
    {
        Managers.Init();

    }

    [ContextMenu("�� �̵�")]
    public void LoadSelectedScene()
    {
        string sceneName = targetScene.ToString();
        Managers.Scene.LoadSceneAsync(sceneName, () =>
        {
            Debug.Log($"[SceneTestUI] �� �ε� �Ϸ�: {sceneName}");
        });
    }

    [ContextMenu("�� �߰�")]
    public void AddSelectedScene()
    {
        string sceneName = targetScene.ToString();
        Managers.Scene.AddSceneAsync(sceneName, () =>
        {
            Debug.Log($"[SceneTestUI] �� �߰� �Ϸ�: {sceneName}");
        });
    }

    [ContextMenu("�� ����")]
    public void UnloadSelectedScene()
    {
        string sceneName = targetScene.ToString();
        Managers.Scene.UnloadSceneAsync(sceneName, () =>
        {
            Debug.Log($"[SceneTestUI] �� ���� �Ϸ�: {sceneName}");
        });
    }
}
