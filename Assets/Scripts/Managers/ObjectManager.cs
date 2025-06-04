using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//������Ʈ�� ���� ����
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
        // �ʱ�ȭ �ʿ�� ����
    }

    public override void Clear()
    {
        base.Clear();
        // �ʱ�ȭ �ʿ�� ����

        //��� ������Ʈ ����
        RemoveAll();
    }

    

    public static Enums.ObjectType GetObjectType(int id)
    {
        var type = (id >> 24) & 0x7f;
        return (Enums.ObjectType)type;
    }

    //id�� ���� ������Ʈ�� ���� ����
    public GameObject Create(Enums.ObjectType type, GameObject prefab, ObjectInform info)
    {
        bool hasPoolable = prefab.GetComponent<Poolable>() != null;

        //�ش� ������Ʈ�� ���丮 ã�� => ���丮 ��ã���� ����
        if (!factories.TryGetValue(type, out var factory))
        {
            Debug.Log($"{type}�� ���丮�� ã�� ����. factories��ũ��Ʈ Ȯ��");
        }

        GameObject go;
        int id = IdGenerator.GenerateId(type);

        if (hasPoolable)
        {
            //Poolable�̶�� Pool���� ��������
            Poolable poolable = Managers.Pool.Pop(prefab); // ���⼭ Ÿ�� ��� prefab �ѱ�
            go = poolable.gameObject;
            go.transform.position = info.Position;
            go.transform.rotation = Quaternion.Euler(info.Rotation);
            go.transform.localScale = info.Scale;
            go.SetActive(true);

            return go;
        }
        else
        {
           // Poolable ������ �׳� ���� ����
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
