using UnityEngine;
using VehiclePhysics;

public class RaycastDetection : MonoBehaviour
{
    private VPStandardInput vpInput;

    public float rayLength = 3f; // ������ ����
    public LayerMask detectionLayer; // ������ ���̾� ����
    public float rayHeightOffset = 0.5f; // Y�� ����
    public float rayBackwardOffset = 3f; // �������� �̵��� �Ÿ�
    private Vector3 rayOrigin; // ����׿� ���� ������
    private Vector3 rayDirection; // ����׿� ���� ����

    private void Start()
    {
        // VPStandardInput �� LaneDetection ������Ʈ�� �����ɴϴ�.
        vpInput = FindObjectOfType<VPStandardInput>();

        if (vpInput == null)
        {
            Debug.LogError("VPStandardInput component not found!");
        }

    }
    void Update()
    {
        // ���� �ӵ� Ȯ��
        float vehicleSpeed = vpInput.vehicle.speed;

        rayLength = (float)(3 + (0.3*vehicleSpeed));
        rayBackwardOffset = -(float)(0.3 * vehicleSpeed);

        // ���� ���� ��ġ�� ���� ����
        rayOrigin = transform.position + Vector3.up * rayHeightOffset - transform.forward * rayBackwardOffset; // �������� ������ �߰�
        rayDirection = transform.forward;

        // ����׿� ���� �ð�ȭ
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);


        if (vehicleSpeed < 1)
        {
            vpInput.externalHandbrake = 0f;
            vpInput.externalBrake = 0f;
            return;
        }

        // ����ĳ��Ʈ ��Ʈ ���� ����
        RaycastHit hitInfo;

        // ����ĳ��Ʈ ����
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, rayLength, detectionLayer))
        {
            // ������ ��ü ���� ���
            Debug.Log("Detected Object: " + hitInfo.collider.gameObject.name);
            // ��ü Ž�� �� �극��ũ ����
            vpInput.externalThrottle = 0f;
            vpInput.externalHandbrake = 1f; // �극��ũ ���� ����
            vpInput.externalBrake = 1f;
            Debug.Log("Obstacle detected! Braking... Speed: "+ vehicleSpeed);

            
            
        }
        else
        {
            // ��ü�� ���� ��
            Debug.Log("No objects detected in front.");
        }

        
    }

    private void OnDrawGizmos()
    {
        // ������ ������ ����
        Gizmos.color = Color.red;

        // ���̸� �ð������� ǥ��
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * rayLength);
    }

}
