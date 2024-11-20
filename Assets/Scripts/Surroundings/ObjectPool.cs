using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // ĳ���� ������
    public int initialPoolSize = 10; // �ʱ� ������ ������Ʈ ��
    private Queue<GameObject> pool = new Queue<GameObject>();

    // Ǯ �ʱ�ȭ
    void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false); // ��Ȱ��ȭ ���·� Ǯ�� �߰�
            pool.Enqueue(obj);
        }
    }

    // ������Ʈ ��������
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // �ʿ� �� �߰� ����
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);
            return obj;
        }
    }

    // ������Ʈ ��ȯ
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
