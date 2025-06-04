using System.Collections;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    public Poolable poolablePrefab; // 프리팹을 Inspector에서 할당
    public int spawnCount = 10;       // 생성할 오브젝트 수
    public float spawnInterval = 0.5f; // 생성 간격
    public float lifeTime = 2f;       // 오브젝트가 존재하는 시간

    private void Start()
    {
        // 풀 매니저 초기화
        PoolManager poolManager = new PoolManager();
        poolManager.Init();

        // 풀 생성
        poolManager.CreatePool(poolablePrefab.gameObject, spawnCount);

        // 테스트 실행
        StartCoroutine(SpawnAndReturnObjects(poolManager));
    }

    private IEnumerator SpawnAndReturnObjects(PoolManager poolManager)
    {
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // 풀에서 꺼내기
                Poolable pooledObj = poolManager.Pop(poolablePrefab.gameObject);
                pooledObj.transform.position = new Vector3(i * 1.5f, 0, 0); // 임의 위치 배치

                // 일정 시간 후 반환
                StartCoroutine(ReturnToPoolAfterSeconds(poolManager, pooledObj, lifeTime));

                yield return new WaitForSeconds(spawnInterval);
            }

            yield return new WaitForSeconds(lifeTime + 1f); // 잠시 쉬었다가 다시 실행
        }
    }

    private IEnumerator ReturnToPoolAfterSeconds(PoolManager poolManager, Poolable obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        poolManager.Push(obj);
    }
}
