using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // ī�޶� �̵� �ӵ�
    public float lookSpeed = 2.0f; // ���콺 ȸ�� �ӵ�
    public float maxLookAngle = 80f; // ī�޶� �� �� �ִ� �ִ� ����

    private float yaw = 0.0f; // ���� ȸ��
    private float pitch = 0.0f; // ���� ȸ��

    void Update()
    {
        // ī�޶� �̵� ó�� (W, A, S, D Ű�� �̵�)
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // �¿� �̵� (A, D)
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // �յ� �̵� (W, S)

        transform.Translate(new Vector3(moveX, 0, moveZ));

        // ���콺 �Է��� ���� ī�޶� ȸ�� ó��
        yaw += Input.GetAxis("Mouse X") * lookSpeed; // ���� ȸ��
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed; // ���� ȸ��

        // ī�޶� �� �� �ִ� ������ ����
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        // ī�޶� ȸ�� ����
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}

