using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void Awake()
    {
        Managers.Init();
    }

    public void Start()
    {
        // 프리팹 경로 검색
        string prefabPath = TypeToPath.GetPath(ObjectType.Test);
        
        // 생성할 위치 배열
        Vector3 position = new Vector3(1, 3, 2);

        // 공통 로테이션과 스케일
        Vector3 rotationEuler = new Vector3(90, 0, 0);
        Vector3 scale = new Vector3(3, 3, 3);

        // ObjectInform 생성
        ObjectInform info = new ObjectInform
        {
            Position = position,
            Rotation = rotationEuler,
            Scale = scale,
            objectType = ObjectType.Player,  // 타입 지정 (예시)
            name = "TestPrefab"
        };

        // 프리팹 로드
        GameObject prefab = Managers.Resource.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"Prefab '{prefabPath}'을(를) 로드하지 못했습니다.");
            return;
        }

        // 생성 호출 
        Managers.Object.Create(info.objectType, prefab, info);
    }
}
