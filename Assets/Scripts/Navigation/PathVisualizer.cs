using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class PathVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private NavMeshAgent agent;
    public float yOffset = 1f;  // NavMesh ������ Y�� ������

    void Start()
    {
        // Line Renderer �� NavMeshAgent ���� ��������
        lineRenderer = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;

        lineRenderer.positionCount = agent.path.corners.Length;
        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            Vector3 position = agent.path.corners[i];
            position.y += yOffset;  // Y�࿡ ������ �߰�
            lineRenderer.SetPosition(i, position);
        }

        // �ʱ� ��� �׸���
        UpdatePathLine();
    }

    void Update()
    {
        // NavMeshAgent�� ��ΰ� ������Ʈ�� ������ ���� �ٽ� �׸�
        if (agent.hasPath)
        {
            UpdatePathLine();
            lineRenderer.positionCount = agent.path.corners.Length;
            for (int i = 0; i < agent.path.corners.Length; i++)
            {
                Vector3 position = agent.path.corners[i];
                position.y += yOffset;  // Y�࿡ ������ �߰�
                lineRenderer.SetPosition(i, position);
            }
        }
    }

    void UpdatePathLine()
    {
        // NavMeshAgent�� ��� ���� ������ Line Renderer�� �Ҵ�
        NavMeshPath path = agent.path;
        lineRenderer.positionCount = path.corners.Length;

        // �� ���� Line Renderer�� ����
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }
    }
}
