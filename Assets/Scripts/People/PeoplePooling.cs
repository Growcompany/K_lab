using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeoplePooling : MonoBehaviour
{
    public GameObject casual_Male_K; // Man ������
    public GameObject casual_Female_K; // Woman ������
    public GameObject MainCar; // MainCar (���� �ִ� ����)
    public int poolSize = 10; // Ǯ���� ��ü ��
    public float spawnRadius = 30.0f; // ��ü�� ������ �ݰ�
    public float objectMoveSpeed = 2.0f; // ��ü�� �̵� �ӵ�

    private List<GameObject> objectPool; // Ǯ���� ��ü ���
    private List<Vector3> relativePositions; // MainCar ���� ��� ��ġ
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

        // Object Pool �� ��� ��ġ ��� �ʱ�ȭ
        objectPool = new List<GameObject>();
        relativePositions = new List<Vector3>();

        for (int i = 0; i < poolSize; i++)
        {
            // Man�� Woman �������� ������ Ǯ��
            GameObject prefabToUse = useManPrefab ? casual_Male_K : casual_Female_K;
            useManPrefab = !useManPrefab; // ���� ������ �ٸ� �������� ���

            GameObject obj = Instantiate(prefabToUse);
            Debug.Log("Instantiated: " + obj.name); // ��ü�� �̸��� ���
            obj.SetActive(false); // �ʱ� ���´� ��Ȱ��ȭ
            objectPool.Add(obj);

            // MainCar �ֺ��� ���� ��ġ ����
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0; // y���� ���� (2D ��鿡�� �̵�)
            relativePositions.Add(randomPosition);
        }
    }

    void Update()
    {
        if (MainCar == null) return; // MainCar�� ������ ������Ʈ �ߴ�

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = objectPool[i];

            // ��ü�� ��Ȱ��ȭ�Ǿ� ������ Ȱ��ȭ
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                Debug.Log("Activating object: " + obj.name); // ��ü �̸� ���
            }

            // MainCar �ֺ����� ��ü �̵� (����� ��ġ ����)
            Vector3 targetPosition = MainCar.transform.position + relativePositions[i];
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, Time.deltaTime * objectMoveSpeed);

            Debug.Log("Object position: " + obj.transform.position); // ��ü ��ġ ���� ���
        }
    }
}
