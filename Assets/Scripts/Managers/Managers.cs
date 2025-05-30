using UnityEngine;

public class Managers : MonoBehaviour
{
    #region ManagerList
    public static PoolManager Pool { get; private set; } = new();
    public static ResourceManager Resource { get; private set; } = new();
    public static ObjectManager Object { get; private set; } = new();
    public static SceneManagerEx Scene { get; private set; } = new();
    public static UIManager UI { get; private set; } = new();
    #endregion

    private static Managers _instance;
    public static Managers Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Managers�� Init()���� �ʾҽ��ϴ�.");
            return _instance;
        }
    }

    public static void Init()
    {
        if (_instance != null)
            return;

        Debug.Log("Managers �ʱ�ȭ ����");
        GameObject managerObj = GameObject.Find("Managers");
        if (managerObj == null)
        {
            managerObj = new GameObject("Managers");
            _instance = managerObj.AddComponent<Managers>();
        }
        else
        {
            _instance = managerObj.GetComponent<Managers>() ?? managerObj.AddComponent<Managers>();
        }

        DontDestroyOnLoad(managerObj);
        _instance.InitManagers();
        Debug.Log("Managers �ʱ�ȭ �Ϸ�");

    }

    public void Clear()
    {
        ClearManagers();
    }

    private void InitManagers()
    {
        Debug.Log("���� �Ŵ��� �ʱ�ȭ ����");
        Pool.Init();
        Resource.Init();
        Object.Init();
        Scene.Init();
        UI.Init();
        Debug.Log("���� �Ŵ��� �ʱ�ȭ �Ϸ�");
    }

    public void ClearManagers()
    {
        Debug.Log("���� �Ŵ��� ���� ����");
        Object.Clear();
        Resource.Clear();
        Pool.Clear();
        Scene.Clear();
        UI.Clear();
        Debug.Log("���� �Ŵ��� ���� �Ϸ�");
    }
}
