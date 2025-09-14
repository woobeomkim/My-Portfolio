using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectDB<T> : MonoBehaviour where T : ScriptableObject
{
    static Dictionary<string, T> objects;

    public static void Init()
    {
        objects = new Dictionary<string, T>();

        // 빈문자열이면 리소시스내 모든디렉토리를찾아서 반호나함
        var objectArray = Resources.LoadAll<T>("");
        foreach (var obj in objectArray)
        {
            // 같은이름이 있으면 오류
            if (objects.ContainsKey(obj.name))
            {
                Debug.LogError($"포켓몬 이름 중복 {obj.name}");
                continue;
            }

            objects[obj.name] = obj;
        }
    }

    public static T GetObjectByName(string name)
    {
        if (!objects.ContainsKey(name))
        {
            Debug.LogError($"오브젝트 데이터베이스에 데이터없음 {name}");
            return null;
        }

        return objects[name];
    }
}
