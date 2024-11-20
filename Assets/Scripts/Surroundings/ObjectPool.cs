using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] prefabs; // ���� ������ ĳ���� ������
    public int initialPoolSize = 10;
    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        foreach (var prefab in prefabs)
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    public GameObject GetObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Ǯ�� ��ü�� ������ ���� ������ ����
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject newObj = Instantiate(prefabs[randomIndex]);
        pool.Add(newObj);
        return newObj;
    }
}
