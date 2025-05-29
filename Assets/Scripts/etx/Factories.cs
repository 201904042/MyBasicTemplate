using UnityEngine;

public class PlayerFactory : IObjectFactory
{
    public GameObject Create(GameObject prefab, ObjectInform info)
    {
        var go = GameObject.Instantiate(prefab);
        go.name = info.name;
        go.transform.position = info.Position;
        go.transform.eulerAngles = info.Rotation;
        go.transform.localScale = info.Scale;
       
        //플레이어 객체 초기화

        return go;
    }
}

public class EnemyFactory : IObjectFactory
{
    public GameObject Create(GameObject prefab, ObjectInform info)
    {
        var go = GameObject.Instantiate(prefab);
        go.name = info.name;
        go.transform.position = info.Position;
        go.transform.eulerAngles = info.Rotation;
        go.transform.localScale = info.Scale;
        
        //에너미 객체 초기화

        return go;
    }
}