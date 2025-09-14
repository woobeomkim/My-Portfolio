using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectDB<T> : MonoBehaviour where T : ScriptableObject
{
    static Dictionary<string, T> objects;

    public static void Init()
    {
        objects = new Dictionary<string, T>();

        // ���ڿ��̸� ���ҽý��� �����丮��ã�Ƽ� ��ȣ����
        var objectArray = Resources.LoadAll<T>("");
        foreach (var obj in objectArray)
        {
            // �����̸��� ������ ����
            if (objects.ContainsKey(obj.name))
            {
                Debug.LogError($"���ϸ� �̸� �ߺ� {obj.name}");
                continue;
            }

            objects[obj.name] = obj;
        }
    }

    public static T GetObjectByName(string name)
    {
        if (!objects.ContainsKey(name))
        {
            Debug.LogError($"������Ʈ �����ͺ��̽��� �����;��� {name}");
            return null;
        }

        return objects[name];
    }
}
