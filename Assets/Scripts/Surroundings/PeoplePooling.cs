using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeoplePooling : MonoBehaviour
{
    public GameObject casual_Male_K; // Man ������
    public GameObject casual_Female_K; // Woman ������
    public GameObject MainCar; // MainCar (���� �ִ� ����)
    public int poolSize = 50; // Ǯ���� ��ü ��
    public float spawnRadius = 5.0f; // ��ü�� ������ �ݰ�
    public float objectMoveSpeed = 2.0f; // ��ü�� �̵� �ӵ�

    private List<GameObject> objectPool; // Ǯ���� ��ü ���
    private bool useManPrefab = true; // Man�� Woman �������� ������ �����ϴ� �÷���

    void Start()
    {
        // MainCar�� ���� �ִ��� Ȯ��
        if (MainCar == null)
        {
            Debug.LogError("MainCar not assigned! Please assign MainCar in the Inspector.");
            return;
        }

        // �������� ����� �Ҵ�Ǿ� �ִ��� Ȯ��
        if (casual_Male_K == null || casual_Female_K == null)
        {
            Debug.LogError("Casual_Male_K or Casual_Female_K prefab not assigned! Please assign them in the Inspector.");
            return;
        }

        // Object Pool �ʱ�ȭ
        objectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            // Man�� Woman �������� ������ Ǯ��
            GameObject prefabToUse = useManPrefab ? casual_Male_K : casual_Female_K;
            useManPrefab = !useManPrefab; // ���� ������ �ٸ� �������� ���

            GameObject obj = Instantiate(prefabToUse);
            obj.SetActive(false); // �ʱ� ���´� ��Ȱ��ȭ
            objectPool.Add(obj);
        }
    }

    void Update()
    {
        if (MainCar == null)
        {
            Debug.LogError("MainCar is missing.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = objectPool[i];

            // ��ü�� ��Ȱ��ȭ�Ǿ� ������ Ȱ��ȭ
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
            }

            // MainCar �ֺ��� ���� ��ǥ ��ġ ����
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0; // y���� ���� (2D ��鿡�� �̵�)
            Vector3 targetPosition = MainCar.transform.position + randomPosition;

            // MoveTowards�� ����� ��ǥ ��ġ�� ���������� �̵�
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, Time.deltaTime * objectMoveSpeed);
        }
    }
}
