using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//오브젝트의 대한 정보
public class ObjectInform
{
    public int id;
    public string name = "null";
    public Enums.ObjectType objectType;
    public Vector3 Position = Vector3.zero;
    public Vector3 Rotation = Vector3.zero;
    public Vector3 Scale = Vector3.one;
}

public class ObjectManager : ManagerBase
{
    private Dictionary<Enums.ObjectType, IObjectFactory> factories = new Dictionary<Enums.ObjectType, IObjectFactory>()
    {
        { Enums.ObjectType.Test, new TestFactory() },
        { Enums.ObjectType.PoolTest, new TestFactory() },
        { Enums.ObjectType.Player, new PlayerFactory() },
        { Enums.ObjectType.Enemy, new EnemyFactory() }
    };

    private readonly Dictionary<int, GameObject> _objects = new();

    public override void Init()
    {
        base.Init();
        // 초기화 필요시 구현
    }

    public override void Clear()
    {
        base.Clear();
        // 초기화 필요시 구현

        //모든 오브젝트 제거
        RemoveAll();
    }

    

    public static Enums.ObjectType GetObjectType(int id)
    {
        var type = (id >> 24) & 0x7f;
        return (Enums.ObjectType)type;
    }

    //id를 가진 오브젝트의 생성 전용
    public GameObject Create(Enums.ObjectType type, GameObject prefab, ObjectInform info)
    {
        bool hasPoolable = prefab.GetComponent<Poolable>() != null;

        //해당 오브젝트의 팩토리 찾기 => 팩토리 못찾으면 에러
        if (!factories.TryGetValue(type, out var factory))
        {
            Debug.Log($"{type}의 팩토리를 찾지 못함. factories스크립트 확인");
        }

        GameObject go;
        int id = IdGenerator.GenerateId(type);

        if (hasPoolable)
        {
            //Poolable이라면 Pool에서 가져오기
            Poolable poolable = Managers.Pool.Pop(prefab); // 여기서 타입 대신 prefab 넘김
            go = poolable.gameObject;
            go.transform.position = info.Position;
            go.transform.rotation = Quaternion.Euler(info.Rotation);
            go.transform.localScale = info.Scale;
            go.SetActive(true);

            return go;
        }
        else
        {
           // Poolable 없으면 그냥 새로 생성
           go = factory.Create(prefab, info);
        }

        _objects.Add(id, go);

        return go;
    }

    public GameObject FindById(int id)
    {
        _objects.TryGetValue(id, out var obj);
        return obj;
    }

    public void Remove(int id)
    {
        if (!_objects.TryGetValue(id, out var obj)) 
            return;

        _objects.Remove(id);

        var poolable = obj.GetComponent<Poolable>();
        if (poolable != null)
            Managers.Pool.Push(poolable);
        else
            Managers.Resource.Destroy(obj);
    }

    private void RemoveAll()
    {
        foreach (var obj in _objects.Values)
        {
            if (obj == null)
                continue;

            var poolable = obj.GetComponent<Poolable>();
            if (poolable != null)
                Managers.Pool.Push(poolable);
            else
                Managers.Resource.Destroy(obj);
        }
        _objects.Clear();
    }



}
