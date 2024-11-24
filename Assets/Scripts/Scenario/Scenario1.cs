using UnityEngine;
using UnityEngine.UI;

public class Scenario1 : MonoBehaviour
{
    public GameObject objectToSpawn; // ������ ������Ʈ
    public Transform vehicleFront; // ������ �� ��ġ
    public Rigidbody vehicleRigidbody; // ������ Rigidbody
    public Toggle spawnToggle; // Toggle UI
    private bool isSpawning = false; // ���� Ȱ��ȭ ����
    private float spawnInterval = 5f; // ���� �ֱ�
    private float spawnTimer = 0f; // Ÿ�̸�
    private float baseSpawnDistance = 2f; // �⺻ ���� �Ÿ�
    private float speedMultiplier = 1f; // �ӵ��� ���� �Ÿ� ����

    void Start()
    {
        // Toggle �� ���� �̺�Ʈ ���
        if (spawnToggle != null)
        {
            spawnToggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    void Update()
    {
        // ���� Ȱ��ȭ �� Ÿ�̸ӷ� ������Ʈ ����
        if (isSpawning)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnObject();
                spawnTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
    }

    private void OnToggleChanged(bool value)
    {
        // Toggle ���� ���� �� ���� Ȱ��ȭ ���� ������Ʈ
        isSpawning = value;
        if (!isSpawning)
        {
            spawnTimer = 0f; // ��Ȱ��ȭ �� Ÿ�̸� �ʱ�ȭ
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null && vehicleFront != null && vehicleRigidbody != null)
        {
            // ������ �ӵ��� ����� �Ÿ� ���
            float speed = vehicleRigidbody.velocity.magnitude; // ���� �ӵ� ũ��
            float dynamicDistance = baseSpawnDistance + (speed * speedMultiplier);

            // ���� ��ġ ���
            Vector3 spawnPosition = vehicleFront.position + vehicleFront.forward * dynamicDistance;

            // ������Ʈ ����
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
