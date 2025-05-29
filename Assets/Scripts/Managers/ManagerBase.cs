using UnityEngine;

public class ManagerBase : IManager
{
    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    public virtual void Init()
    {
        Debug.Log($"{GetType().Name} �ʱ�ȭ �Ϸ�");
    }

    /// <summary>
    /// ���ҽ� ���� �Ǵ� ������
    /// </summary>
    public virtual void Clear()
    {
        Debug.Log($"{GetType().Name} Ŭ���� �Ϸ�");
    }
}
