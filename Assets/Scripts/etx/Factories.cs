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
       
        //�÷��̾� ��ü �ʱ�ȭ

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
        
        //���ʹ� ��ü �ʱ�ȭ

        return go;
    }
}