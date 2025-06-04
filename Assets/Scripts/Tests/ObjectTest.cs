using UnityEngine;

public class ObjectTest : MonoBehaviour
{
    [Header("������ ������Ʈ Ÿ�� ����")]
    public Enums.ObjectType spawnType = Enums.ObjectType.Player;

    [Header("���� ��ġ")]
    public Vector3 spawnPosition = new Vector3(1, 3, 2);

    [Header("���� ȸ�� (Euler)")]
    public Vector3 spawnRotation = new Vector3(90, 0, 0);

    [Header("���� ������")]
    public Vector3 spawnScale = new Vector3(3, 3, 3);

    [Header("���� ����")]
    [Min(1)]
    public int spawnCount = 1;

    public void Awake()
    {
        Managers.Init();
    }

    [ContextMenu("����")]
    public void SpawnTest()
    {
        string prefabPath = TypeToPath.GetPath(spawnType);

        GameObject prefab = Managers.Resource.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab '{prefabPath}'��(��) �ε����� ���߽��ϴ�.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // ��ġ �ణ�� �ٸ��� �ϱ� ���� ��: x������ i��ŭ ������ �߰�
            Vector3 offsetPosition = spawnPosition + new Vector3(i, 0, 0);

            ObjectInform info = new ObjectInform
            {
                Position = offsetPosition,
                Rotation = spawnRotation,
                Scale = spawnScale,
                objectType = spawnType,
                name = spawnType.ToString() + $"_Prefab_{i}"
            };

            Managers.Object.Create(info.objectType, prefab, info);
        }
    }

    [ContextMenu("���� ����")]
    public void DestroyAll()
    {
        Managers.Object.Clear();
    
    }
}
