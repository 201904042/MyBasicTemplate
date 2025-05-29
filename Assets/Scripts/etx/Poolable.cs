using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool IsUsing { get; set; } = false;

    /// <summary>
    /// Ǯ���� ���� �� �ʱ�ȭ ����
    /// </summary>
    public virtual void OnPop()
    {
        // �ʿ�� �ʱ�ȭ �ڵ� ����
    }

    /// <summary>
    /// Ǯ�� ��ȯ�� �� ���� ����
    /// </summary>
    public virtual void OnPush()
    {
        // �ʿ�� ���� �ڵ� ����
    }
}
