using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousVehicleController1 : MonoBehaviour
{
    public float maxSpeed = 15f;  // ������ �ִ� �ӵ�
    public float acceleration = 4f;  // ���ӵ�
    public float deceleration = 6f;  // ���ӵ�
    public float turnSpeed = 2f;
    public float maxSteerAngle = 30f; // �ִ� ���� ���� (�չ����� �¿�� ȸ���ϴ� ����)
    public float wheelRotationSpeed = 720f; // ���� ȸ�� �ӵ�

    private Transform wheelFL;  // �� ���� ����
    private Transform wheelFR;  // �� ������ ����
    private Transform wheelRL;  // �� ���� ����
    private Transform wheelRR;  // �� ������ ����
    private Rigidbody rb;
    private float currentSpeed = 0f;  // ���� �ӵ�
    private float currentSteerAngle = 0f; // ���� ���Ⱒ

    void Start()
    {
        // ���� ã��
        wheelFL = transform.Find("Wheel_FL");
        wheelFR = transform.Find("Wheel_FR");
        wheelRL = transform.Find("Wheel_RL");
        wheelRR = transform.Find("Wheel_RR");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveVehicle();
    }

    public void SetSteering(float steeringAngle)
    {
        // ������ ���� ���� �ڵ� ���� ���� �޾� ����
        currentSteerAngle = Mathf.Clamp(steeringAngle, -maxSteerAngle, maxSteerAngle);
    }

    void MoveVehicle()
    {
        float moveInput = 1f; // �ڵ� ����
        float steer = currentSteerAngle;

        // �չ����� ���� ���� ����
        wheelFL.localRotation = Quaternion.Euler(0, steer, 0);
        wheelFR.localRotation = Quaternion.Euler(0, steer, 0);

        // �ӵ� ��� (����/���� ó��)
        currentSpeed += moveInput * acceleration * Time.deltaTime; // ���ӵ� ����
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed); // �ִ� �ӵ� ����

        // ���� �̵� ó�� (Rigidbody�� velocity�� ���� �ӵ� ����)
        Vector3 moveDirection = transform.forward * currentSpeed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);

        // ���� ȸ�� ó�� (�ӵ��� ���� ȸ�� ���� ����)
        if (currentSpeed != 0)
        {
            float turnAmount = steer * turnSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turnAmount, 0));
        }

        // ���� ���� ó��
        RotateWheels(moveInput);
    }

    void RotateWheels(float moveInput)
    {
        // ������ ������ ����/������ �� ������ ȸ��
        if (moveInput != 0f)
        {
            // ������ ȸ�� ���� ���
            float wheelTurnAngle = moveInput * wheelRotationSpeed * Time.deltaTime;
            // ������ �������� ȸ�� ����
            wheelFL.Rotate(Vector3.right, wheelTurnAngle);
            wheelFR.Rotate(Vector3.right, wheelTurnAngle);
            wheelRL.Rotate(Vector3.right, wheelTurnAngle);
            wheelRR.Rotate(Vector3.right, wheelTurnAngle);
        }
    }
}