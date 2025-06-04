using System.Collections.Generic;

/*
 * 아이디는 32비트의 정수를 담으며 상위 8비트는 오브젝트 타입, 하위 24비트는 인덱스를 의미
 * 
 * 그렇기에 ObjectType의 enum은 최대 256개로 생성이 가능하며
 * 가능한 인덱스는 1,677만 개 생성이 가능함.
 * 
 * 또한 삭제된 인덱스는 재사용이 가능하도록 설계
 */

public static class IdGenerator
{
    private static readonly Dictionary<Enums.ObjectType, Queue<int>> _recycled = new();
    private static readonly Dictionary<Enums.ObjectType, int> _counters = new();

    /// <summary>
    /// 타입별로 고유한 아이디를 생성
    /// </summary>
    public static int GenerateId(Enums.ObjectType type)
    {
        if (!_recycled.ContainsKey(type))
            _recycled[type] = new Queue<int>();
        if (!_counters.ContainsKey(type))
            _counters[type] = 1;

        int index;
        if (_recycled[type].Count > 0)
        {
            index = _recycled[type].Dequeue(); // 재사용 ID
        }
        else
        {
            index = _counters[type]++;
        }

        return ((int)type << 24) | (index & 0xFFFFFF);
    }

    public static void RecycleId(int id)
    {
        Enums.ObjectType type = (Enums.ObjectType)((id >> 24) & 0x7F);
        int index = id & 0xFFFFFF;

        if (!_recycled.ContainsKey(type))
            _recycled[type] = new Queue<int>();

        _recycled[type].Enqueue(index);
    }
}
