using UnityEngine;
using TrafficSimulation;

public class ObstacleCarController : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Trigger radius for detecting the main car")]
    public float detectionRadius = 20f; // ���� �ݰ�

    [Tooltip("Reference to the main car")]
    public Transform mainCar; // ���� ������ Transform�� Inspector���� ���� ����

    private VehicleAI vehicleAI; // VehicleAI ����
    private WheelDrive wheelDrive; // WheelDrive ����
    private bool isActivated = false; // ������ Ȱ��ȭ �������� Ȯ��

    void Start()
    {
        // VehicleAI �� WheelDrive ������Ʈ ã��
        vehicleAI = GetComponent<VehicleAI>();
        wheelDrive = GetComponent<WheelDrive>();

        if (vehicleAI == null || wheelDrive == null)
        {
            Debug.LogError("VehicleAI or WheelDrive component not found!");
            return;
        }

        // �ʱ� ���·� ��Ȱ��ȭ
        vehicleAI.enabled = false;
        wheelDrive.enabled = false;
    }

    void Update()
    {
        // �̹� Ȱ��ȭ�� ��� �� �̻� ���� ���� ���� �� ��
        if (isActivated || mainCar == null)
            return;

        // ���� �������� �Ÿ� ���
        float distanceToMainCar = Vector3.Distance(transform.position, mainCar.position);

        // �ݰ� ���� ���� ������ ������ VehicleAI �� WheelDrive Ȱ��ȭ
        if (distanceToMainCar <= detectionRadius)
        {
            isActivated = true;
            Debug.Log("Main car detected! Activating obstacle car.");
            ActivateVehicle();
        }
    }

    // VehicleAI �� WheelDrive Ȱ��ȭ
    private void ActivateVehicle()
    {
        if (vehicleAI != null) vehicleAI.enabled = true;
        if (wheelDrive != null) wheelDrive.enabled = true;
        Debug.Log("VehicleAI and WheelDrive activated.");
    }
}
