using System.Collections.Generic;

/*
 * ���̵�� 32��Ʈ�� ������ ������ ���� 8��Ʈ�� ������Ʈ Ÿ��, ���� 24��Ʈ�� �ε����� �ǹ�
 * 
 * �׷��⿡ ObjectType�� enum�� �ִ� 256���� ������ �����ϸ�
 * ������ �ε����� 1,677�� �� ������ ������.
 * 
 * ���� ������ �ε����� ������ �����ϵ��� ����
 */

public static class IdGenerator
{
    private static readonly Dictionary<Enums.ObjectType, Queue<int>> _recycled = new();
    private static readonly Dictionary<Enums.ObjectType, int> _counters = new();

    /// <summary>
    /// Ÿ�Ժ��� ������ ���̵� ����
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
            index = _recycled[type].Dequeue(); // ���� ID
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
