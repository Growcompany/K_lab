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
        float moveX = 0f;
        float moveZ = 0f;

        // ����Ű �Է� ����
        if (Input.GetKey(KeyCode.LeftArrow))  // ���� ����Ű
        {
            moveX = -moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow)) // ������ ����Ű
        {
            moveX = moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))    // ���� ����Ű
        {
            moveZ = moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))  // �Ʒ��� ����Ű
        {
            moveZ = -moveSpeed * Time.deltaTime;
        }

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

