using System;
using System.Collections.Generic;
using UnityEngine;

//������Ʈ�� ���� ����
public class ObjectInform
{
    public int id;
    public string name = "null";
    public ObjectType objectType;
    public Vector3 Position = Vector3.zero;
    public Vector3 Rotation = Vector3.zero;
    public Vector3 Scale = Vector3.one;
}

public class ObjectManager : ManagerBase
{
    private Dictionary<ObjectType, IObjectFactory> factories = new Dictionary<ObjectType, IObjectFactory>()
    {
        { ObjectType.Player, new PlayerFactory() },
        { ObjectType.Enemy, new EnemyFactory() }
    };

    private readonly Dictionary<int, GameObject> _objects = new();

    public override void Init()
    {
        base.Init();
        // �ʱ�ȭ �ʿ�� ����

        
    }

    public static ObjectType GetObjectType(int id)
    {
        var type = (id >> 24) & 0x7f;
        return (ObjectType)type;
    }

    //id�� ���� ������Ʈ�� ���� ����
    public void Create(ObjectType type, string prefabPath, ObjectInform info)
    {
        GameObject prefab = Managers.Resource.Load<GameObject>($"{prefabPath}");
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found: {prefabPath}");
            return;
        }

        if (!factories.TryGetValue(type, out var factory))
            throw new Exception($"No factory registered for type {type}");

        var go = factory.Create(prefab, info); // ���� ��ġ, ȸ�� ����
        int id = IdGenerator.GenerateId(type);
        _objects.Add(id, go);
    }

    public void Create(ObjectType type, GameObject prefab, ObjectInform info)
    {
        if (!factories.TryGetValue(type, out var factory))
            throw new Exception($"No factory registered for type {type}");

        var go = factory.Create(prefab, info);
        int id = IdGenerator.GenerateId(type);
        _objects.Add(id, go);
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

    

}
