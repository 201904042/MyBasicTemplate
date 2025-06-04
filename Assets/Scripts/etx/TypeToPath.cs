using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ÿ�Կ� ���� ���ҽ������� ���
public static class TypeToPath
{
    private static readonly Dictionary<Type, IDictionary> _typeToPathMap = new()
    {
        { typeof(Enums.UIType), new Dictionary<Enums.UIType, string>
            {
                { Enums.UIType.Test, "Prefabs/UI/TestUI" },
                { Enums.UIType.Loading, "Prefabs/UI/TestUI" },
            }
        },
        { typeof(Enums.ObjectType), new Dictionary<Enums.ObjectType, string>
            {
                { Enums.ObjectType.Test, "Prefabs/Object/TestPrefab" },
                { Enums.ObjectType.PoolTest, "Prefabs/Object/PoolableTestPrefab" },
                { Enums.ObjectType.Player, "Prefabs/Object/Player" }
            }
        },
    };

    public static string GetPath<T>(T enumKey) where T : Enum
    {
        if (!_typeToPathMap.TryGetValue(typeof(T), out var dictObj))
        {
            Debug.LogError($"[TypeToPath] {typeof(T).Name} Ÿ�Կ� ���� ��� ��ųʸ��� ��ϵǾ� ���� �ʽ��ϴ�.");
            return null;
        }

        var dict = dictObj as Dictionary<T, string>;
        if (dict == null || !dict.TryGetValue(enumKey, out var path))
        {
            Debug.LogError($"[TypeToPath] {enumKey} �� ��ΰ� �������� �ʽ��ϴ�.");
            return null;
        }

        return path;
    }
}
