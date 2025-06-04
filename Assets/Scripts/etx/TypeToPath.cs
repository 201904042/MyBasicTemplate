using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타입에 따른 리소스이후의 경로
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
            Debug.LogError($"[TypeToPath] {typeof(T).Name} 타입에 대한 경로 딕셔너리가 등록되어 있지 않습니다.");
            return null;
        }

        var dict = dictObj as Dictionary<T, string>;
        if (dict == null || !dict.TryGetValue(enumKey, out var path))
        {
            Debug.LogError($"[TypeToPath] {enumKey} 의 경로가 존재하지 않습니다.");
            return null;
        }

        return path;
    }
}
