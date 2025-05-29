using UnityEngine;

public class ManagerBase : IManager
{
    /// <summary>
    /// 초기화 메서드
    /// </summary>
    public virtual void Init()
    {
        Debug.Log($"{GetType().Name} 초기화 완료");
    }

    /// <summary>
    /// 리소스 해제 또는 정리용
    /// </summary>
    public virtual void Clear()
    {
        Debug.Log($"{GetType().Name} 클리어 완료");
    }
}
