using UnityEngine;

public interface IObjectFactory
{
    GameObject Create(GameObject prefab, ObjectInform info);
}

public interface IManager
{
    void Init();
    void Clear();
}
