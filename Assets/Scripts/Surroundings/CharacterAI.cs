using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    private Transform[] waypoints; // �ڽ� Waypoints �迭
    private int currentWaypointIndex = 0;

    public float minDistance = 1.0f; // Waypoint ���� ���� �Ÿ�
    public float speed = 3.0f;

    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
    }

    public void SetWaypointsParent(GameObject waypointParent)
    {
        // Waypoint Parent���� ��� �ڽ� Transform ��������
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

        isMoving = true;
        currentWaypointIndex = Random.Range(0, waypoints.Length); // ���� ���� ����
        agent.isStopped = false; // NavMeshAgent �̵� ���
        agent.SetDestination(waypoints[currentWaypointIndex].position); // ù ������ ����
    }

    void Update()
    {
        if (isMoving && waypoints != null && waypoints.Length > 0)
        {
            MoveToWaypoint();
            UpdateAnimation();
        }
    }

    private void MoveToWaypoint()
    {
        // ���� Waypoint�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < minDistance)
        {
            // ���� Waypoint�� �̵�
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void UpdateAnimation()
    {
        // NavMeshAgent�� �ӵ��� ����ȭ�Ͽ� "vertical" �Ķ���� ����
        float velocity = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("vertical", velocity); // �ִϸ��̼� ���� Ʈ���� vertical ������Ʈ
    }

    public void StopMoving()
    {
        isMoving = false;
        agent.isStopped = true;
        animator.SetFloat("vertical", 0f); // ���� �ִϸ��̼�
    }
}
