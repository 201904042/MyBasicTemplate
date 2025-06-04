using UnityEngine;

public class ObjectTest : MonoBehaviour
{
    [Header("생성할 오브젝트 타입 선택")]
    public Enums.ObjectType spawnType = Enums.ObjectType.Player;

    [Header("스폰 위치")]
    public Vector3 spawnPosition = new Vector3(1, 3, 2);

    [Header("스폰 회전 (Euler)")]
    public Vector3 spawnRotation = new Vector3(90, 0, 0);

    [Header("스폰 스케일")]
    public Vector3 spawnScale = new Vector3(3, 3, 3);

    [Header("스폰 개수")]
    [Min(1)]
    public int spawnCount = 1;

    public void Awake()
    {
        Managers.Init();
    }

    [ContextMenu("스폰")]
    public void SpawnTest()
    {
        string prefabPath = TypeToPath.GetPath(spawnType);

        GameObject prefab = Managers.Resource.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab '{prefabPath}'을(를) 로드하지 못했습니다.");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // 위치 약간씩 다르게 하기 위해 예: x축으로 i만큼 오프셋 추가
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

    [ContextMenu("전부 삭제")]
    public void DestroyAll()
    {
        Managers.Object.Clear();
    
    }
}
