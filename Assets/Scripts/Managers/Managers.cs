using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    public static Managers Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Managers�� Init()���� �ʾҽ��ϴ�.");
            }
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
            _instance = managerObj.GetComponent<Managers>();
            if (_instance == null)
            {
                _instance = managerObj.AddComponent<Managers>();
            }
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
        // ���� �Ŵ��� �ʱ�ȭ ������
        Debug.Log("���� �Ŵ��� �ʱ�ȭ �Ϸ�");
    }

    public void ClearManagers()
    {
        Debug.Log("���� �Ŵ��� ���� ����");
        // ���� �Ŵ��� ���� ������
        Debug.Log("���� �Ŵ��� ���� �Ϸ�");
    }
}
