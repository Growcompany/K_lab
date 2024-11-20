using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    public Transform[] waypoints; // �̵��� Waypoint �迭
    private int currentWaypointIndex = 0;
    public float speed = 3.0f;

    private bool isMoving = false;

    public void StartMoving()
    {
        isMoving = true;
        currentWaypointIndex = 0;
    }

    void Update()
    {
        if (!isMoving) return;

        // ���� Waypoint�� �̵�
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Waypoint�� ���� �� ���� Waypoint��
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �ݰ� ������ ������ Ǯ�� ��ȯ
        if (other.CompareTag("MainCarRadius"))
        {
            FindObjectOfType<ObjectPool>().ReturnObject(gameObject);
            isMoving = false;
        }
    }
}
