using UnityEngine;

public class ResourceManager : ManagerBase
{
    public override void Init()
    {
        base.Init();
        // 리소스 매니저 초기화
    }

    public override void Clear()
    {
        base.Clear();
        // 리소스 해제 처리
    }

    public T Load<T>(string path) where T : Object
    {
        // 게임 오브젝트면 풀 원본 우선 조회
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
            Debug.LogError($"해당 {path}의 오브젝트를 찾을 수 없음 ");
        }

        return res;
    }

    //id가 필요하지 않은 배경, UI등의 생성용
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
