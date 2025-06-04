using System.Collections;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    public Poolable poolablePrefab; // �������� Inspector���� �Ҵ�
    public int spawnCount = 10;       // ������ ������Ʈ ��
    public float spawnInterval = 0.5f; // ���� ����
    public float lifeTime = 2f;       // ������Ʈ�� �����ϴ� �ð�

    private void Start()
    {
        // Ǯ �Ŵ��� �ʱ�ȭ
        PoolManager poolManager = new PoolManager();
        poolManager.Init();

        // Ǯ ����
        poolManager.CreatePool(poolablePrefab.gameObject, spawnCount);

        // �׽�Ʈ ����
        StartCoroutine(SpawnAndReturnObjects(poolManager));
    }

    private IEnumerator SpawnAndReturnObjects(PoolManager poolManager)
    {
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // Ǯ���� ������
                Poolable pooledObj = poolManager.Pop(poolablePrefab.gameObject);
                pooledObj.transform.position = new Vector3(i * 1.5f, 0, 0); // ���� ��ġ ��ġ

                // ���� �ð� �� ��ȯ
                StartCoroutine(ReturnToPoolAfterSeconds(poolManager, pooledObj, lifeTime));

                yield return new WaitForSeconds(spawnInterval);
            }

            yield return new WaitForSeconds(lifeTime + 1f); // ��� �����ٰ� �ٽ� ����
        }
    }

    private IEnumerator ReturnToPoolAfterSeconds(PoolManager poolManager, Poolable obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        poolManager.Push(obj);
    }
}
