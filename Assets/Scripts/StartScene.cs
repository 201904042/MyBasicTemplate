using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void Awake()
    {
        Managers.Init();
    }

    public void Start()
    {
        // ������ ��� �˻�
        string prefabPath = TypeToPath.GetPath(ObjectType.Test);
        
        // ������ ��ġ �迭
        Vector3 position = new Vector3(1, 3, 2);

        // ���� �����̼ǰ� ������
        Vector3 rotationEuler = new Vector3(90, 0, 0);
        Vector3 scale = new Vector3(3, 3, 3);

        // ObjectInform ����
        ObjectInform info = new ObjectInform
        {
            Position = position,
            Rotation = rotationEuler,
            Scale = scale,
            objectType = ObjectType.Player,  // Ÿ�� ���� (����)
            name = "TestPrefab"
        };

        // ������ �ε�
        GameObject prefab = Managers.Resource.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"Prefab '{prefabPath}'��(��) �ε����� ���߽��ϴ�.");
            return;
        }

        // ���� ȣ�� 
        Managers.Object.Create(info.objectType, prefab, info);
    }
}
