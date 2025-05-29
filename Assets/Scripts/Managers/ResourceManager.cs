using UnityEngine;

public class ResourceManager : ManagerBase
{
    public override void Init()
    {
        base.Init();
        // ���ҽ� �Ŵ��� �ʱ�ȭ
    }

    public override void Clear()
    {
        base.Clear();
        // ���ҽ� ���� ó��
    }

    public T Load<T>(string path) where T : Object
    {
        // ���� ������Ʈ�� Ǯ ���� �켱 ��ȸ
        if (typeof(T) == typeof(GameObject))
        {
            var name = path;
            var index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            var go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        T res = Resources.Load<T>($"{path}");
        if (res == null)
        {
            Debug.LogError($"�ش� {path}�� ������Ʈ�� ã�� �� ���� ");
        }

        return res;
    }

    //id�� �ʿ����� ���� ���, UI���� ������
    public GameObject Instantiate(string path, Transform parent = null)
    {
        var original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.LogWarning($"[ResourceManager] Failed to load prefab: {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        var go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;

        var poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
