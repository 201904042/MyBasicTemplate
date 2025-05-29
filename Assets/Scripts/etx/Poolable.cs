using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool IsUsing { get; set; } = false;

    /// <summary>
    /// 풀에서 꺼낼 때 초기화 로직
    /// </summary>
    public virtual void OnPop()
    {
        // 필요시 초기화 코드 삽입
    }

    /// <summary>
    /// 풀에 반환될 때 정리 로직
    /// </summary>
    public virtual void OnPush()
    {
        // 필요시 정리 코드 삽입
    }
}
