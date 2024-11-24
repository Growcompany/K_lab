using UnityEngine;
using UnityEngine.AI;

public class SurroundCarAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    private Transform[] waypoints; // Waypoints �迭
    private int currentWaypointIndex = 0;

    public float minDistance = 1.0f; // Waypoint ���� ���� �Ÿ�
    public float speed = 3.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
    }

    public void SetWaypointsParent(GameObject waypointParent)
    {
        // WaypointParent���� ��� �ڽ� Waypoints ��������
        Transform parentTransform = waypointParent.transform;
        waypoints = new Transform[parentTransform.childCount];
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            waypoints[i] = parentTransform.GetChild(i);
        }
    }

    public void StartMoving()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        currentWaypointIndex = Random.Range(0, waypoints.Length); // ���� ���� ����
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypointIndex].position); // ù ������ ����
    }

    void Update()
    {
        if (agent.isStopped || waypoints == null || waypoints.Length == 0) return;

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < minDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        float velocity = agent.velocity.magnitude / agent.speed; // ����ȭ �ӵ�
        animator.SetFloat("vertical", velocity);
    }
}
