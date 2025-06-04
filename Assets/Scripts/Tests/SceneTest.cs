using UnityEngine;
using UnityEngine.UI;

public class SceneTest : MonoBehaviour
{
    [Header("Å×½ºÆ®¿ë ¾À ¼±ÅÃ")]
    public Enums.Scene targetScene;

    private void Awake()
    {
        Managers.Init();

    }

    [ContextMenu("¾À ÀÌµ¿")]
    public void LoadSelectedScene()
    {
        string sceneName = targetScene.ToString();
        Managers.Scene.LoadSceneAsync(sceneName, () =>
        {
            Debug.Log($"[SceneTestUI] ¾À ·Îµå ¿Ï·á: {sceneName}");
        });
    }

    [ContextMenu("¾À Ãß°¡")]
    public void AddSelectedScene()
    {
        string sceneName = targetScene.ToString();
        Managers.Scene.AddSceneAsync(sceneName, () =>
        {
            Debug.Log($"[SceneTestUI] ¾À Ãß°¡ ¿Ï·á: {sceneName}");
        });
    }

    [ContextMenu("¾À Á¦°Å")]
    public void UnloadSelectedScene()
    {
        string sceneName = targetScene.ToString();
        Managers.Scene.UnloadSceneAsync(sceneName, () =>
        {
            Debug.Log($"[SceneTestUI] ¾À Á¦°Å ¿Ï·á: {sceneName}");
        });
    }
}
