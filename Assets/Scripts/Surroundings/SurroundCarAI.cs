using UnityEngine;
using UnityEngine.AI;

public class SurroundCarAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform[] waypoints; // Waypoints �迭
    private int currentWaypointIndex = 0;

    public float minDistance = 1.0f; // Waypoint ���� ���� �Ÿ�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    public void SetWaypointsParent(GameObject waypointParent)
    {
        if (waypointParent == null)
        {
            Debug.LogError("WaypointParent�� null�Դϴ�. Waypoints �ʱ�ȭ ����!");
            return;
        }

        Transform parentTransform = waypointParent.transform;
        int childCount = parentTransform.childCount;

        if (childCount == 0)
        {
            Debug.LogError("WaypointParent�� �ڽ� Waypoints�� �����ϴ�!");
            waypoints = null; // Waypoints �ʱ�ȭ ���� ó��
            return;
        }

        waypoints = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            waypoints[i] = parentTransform.GetChild(i);
        }

        Debug.Log($"Waypoints �ʱ�ȭ ����: {childCount}���� Waypoints�� �����Ǿ����ϴ�.");
    }

    public void StartMoving()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints�� �������� �ʾҽ��ϴ�. �̵� �Ұ�!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"NavMeshAgent�� NavMesh ���� ��ġ���� �ʾҽ��ϴ�! {gameObject.name}");
            return;
        }

        currentWaypointIndex = Random.Range(0, waypoints.Length); // ���� ���� ����
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypointIndex].position); // ù ������ ����
    }

    void Update()
    {
        if (agent == null || waypoints == null || waypoints.Length == 0 || agent.isStopped)
        {
            return;
        }

        // Waypoint�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < minDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
