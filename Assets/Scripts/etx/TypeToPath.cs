using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypeToPath
{
    private static readonly Dictionary<Type, IDictionary> _typeToPathMap = new()
    {
        { typeof(UIType), new Dictionary<UIType, string>
            {
                { UIType.Test, "Prefabs/UI/TestUI" },
            }
        },
        { typeof(ObjectType), new Dictionary<ObjectType, string>
            {
                { ObjectType.Test, "Prefabs/Object/TestPrefab" },
                { ObjectType.Player, "Prefabs/Object/Player" }
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
