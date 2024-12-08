using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleController : MonoBehaviour
{
    public float moveDistance = 50f;  // �̵� �Ÿ�
    public float moveSpeed = 5f;     // �̵� �ӵ�
    public float turnSpeed = 360f;   // ȸ�� �ӵ�

    private bool isMovingForward = true;

    void Start()
    {
        StartCoroutine(MoveAndTurnRoutine());
    }

    IEnumerator MoveAndTurnRoutine()
    {
        while (true)
        {
            // ������ �̵�
            yield return StartCoroutine(MoveForward());

            // 180�� ȸ��
            yield return StartCoroutine(TurnAround());
        }
    }

    IEnumerator MoveForward()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * moveDistance;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }

    IEnumerator TurnAround()
    {
        float targetAngle = transform.eulerAngles.y + 180f;
        float currentAngle = transform.eulerAngles.y;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) > 1f)
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }
}
