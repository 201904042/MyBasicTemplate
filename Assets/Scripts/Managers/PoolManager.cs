using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ManagerBase
{
    private readonly Dictionary<string, Pool> _pools = new();
    private Transform _root;

    public override void Init()
    {
        base.Init();
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public override void Clear()
    {
        base.Clear();

        foreach (Transform child in _root)
            Object.Destroy(child.gameObject);

        _pools.Clear();
    }

    /// <summary>
    /// 기존 풀 없으면 생성 (초기 count는 5개)
    /// </summary>
    public void CreatePool(GameObject original, int count = 5)
    {
        if (original == null) return;

        if (_pools.ContainsKey(original.name))
            return; // 이미 생성됨

        var pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pools.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        var name = poolable.gameObject.name;
        if (!_pools.TryGetValue(name, out var pool))
        {
            // 풀에 없으면 그냥 파괴
            Object.Destroy(poolable.gameObject);
            return;
        }

        pool.Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (original == null)
            return null;

        if (!_pools.ContainsKey(original.name))
            CreatePool(original);

        var pool = _pools[original.name];
        var poolable = pool.Pop(parent);
        poolable.OnPop();

        return poolable;
    }

    public GameObject GetOriginal(string name)
    {
        if (_pools.TryGetValue(name, out var pool))
            return pool.Original;
        return null;
    }

    

    private class Pool
    {
        private readonly Stack<Poolable> _poolStack = new();
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject($"{original.name}_PoolRoot").transform;

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        private Poolable Create()
        {
            var go = Object.Instantiate(Original);
            go.name = Original.name;
            var poolable = go.GetComponent<Poolable>();
            if (poolable == null)
                poolable = go.AddComponent<Poolable>();
            return poolable;
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.OnPush();

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //if (parent == null)
            //{
            //    // 기본 부모를 씬 내 현재 씬 루트 등으로 지정 가능
            //    var currentSceneRoot = Managers.Scene.CurrentScene?.transform;
            //    poolable.transform.SetParent(currentSceneRoot);
            //}
            //else
            //    poolable.transform.SetParent(parent);
            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }
}
