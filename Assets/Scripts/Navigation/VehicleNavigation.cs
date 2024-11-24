using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleNavigation : MonoBehaviour
{
    public Transform waypointsParent; // ���������� �ִ� �θ� ������Ʈ
    private List<Transform> allWaypoints = new List<Transform>(); // �ڽ� ������ ����Ʈ
    private List<Transform> selectedWaypoints1 = new List<Transform>();
    private int Left_Waypoints = 0;
    private Transform finalDestination; // ���� ������
    public float minDistance = 10f; // ���������� �ּ� �Ÿ� ����
    private float waypointDetectionRadius = 15f; // ��ο� ������ ������ �ּ� �Ÿ� ����
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0; // ���� ��ǥ ������ �ε���
    public LineRenderer vehiclePathLineRenderer; // ������ ���� ��ġ���� ���� ������������ ��θ� ǥ���� LineRenderer
    public LineRenderer waypointsPathLineRenderer; // ������ �������� ���� ��θ� ǥ���� LineRenderer
    private float minDistanceFromVehicle = 15.0f; // ������ ������ ������ �ּ� �Ÿ� ����
    private Coroutine pathUpdateCoroutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // �θ� ������Ʈ�� ��� �ڽ��� ������ ����Ʈ�� �߰�
        if (waypointsParent != null)
        {
            foreach (Transform child in waypointsParent)
            {
                allWaypoints.Add(child);
            }
        }

        SetupLineRenderer(vehiclePathLineRenderer);
        SetupLineRenderer(waypointsPathLineRenderer);
    }


    private void SetupLineRenderer(LineRenderer lineRenderer)
    {
        // LineRenderer �⺻ ����
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 7f;
        lineRenderer.endWidth = 7f;
        lineRenderer.alignment = LineAlignment.View; // ī�޶� ���߾� ����
    }

    // �������� �����ϴ� �Լ�
    public void SetDestination(Transform newDestination)
    {
        if (vehiclePathLineRenderer != null)
        {
            vehiclePathLineRenderer.enabled = true;
        }

        if (waypointsPathLineRenderer != null)
        {
            waypointsPathLineRenderer.enabled = true;
        }
        finalDestination = newDestination;
        UpdatePath(); // �ʱ� ��� ����
        DrawWaypointsPath(); // �������� ���� ��θ� ����
        StartPathUpdateRoutine(); // �ݺ� �۾��� �ڷ�ƾ���� ����
    }

    private void StartPathUpdateRoutine()
    {
        if (pathUpdateCoroutine != null)
        {
            StopCoroutine(pathUpdateCoroutine);
        }
        pathUpdateCoroutine = StartCoroutine(PathUpdateRoutine());
    }

    private IEnumerator PathUpdateRoutine()
    {
        while (true)
        {
            if (Left_Waypoints > 0)
            {
                UpdatePath();
                DrawWaypointsPath();
            }
            else
            {
                Debug.Log("No waypoints left. Path updates paused.");
                yield break; // �ڷ�ƾ ����
            }

            yield return new WaitForSeconds(1f); // 3�ʸ��� ����
        }
    }

    private void Update()
    {

        if (Left_Waypoints > 0)
        {
            Transform targetWaypoint = selectedWaypoints1[0];
            agent.SetDestination(targetWaypoint.position);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < minDistance)
            {
                UpdatePath(); // ��� ������Ʈ
                DrawWaypointsPath(); // �������� ���� ��� �ٽ� �׸���
                Debug.Log("Arrived!!!!!!!!!");
            }
        }
        else if (Left_Waypoints == 0)
        {
            // ���� �������� ������ ��� LineRenderer ��Ȱ��ȭ
            Debug.Log("Final destination reached. LineRenderers disabled.");
            DisableLineRenderers();
        }
    }

    private void DisableLineRenderers()
    {
        if (vehiclePathLineRenderer != null)
        {
            vehiclePathLineRenderer.enabled = false;
        }

        if (waypointsPathLineRenderer != null)
        {
            waypointsPathLineRenderer.enabled = false;
        }
    }


    // �������� ���� ������������ ��θ� ������Ʈ�ϴ� �Լ�
    private void UpdatePath()
    {
        selectedWaypoints1.Clear(); // ���� ����Ʈ�� ���

        Vector3 nextWaypoint = finalDestination.position;
        // ������ NavMeshAgent�� ������ ����� �� �ڳʸ� Ȯ���ϰ�, ��� ��ó�� �������� ����
        NavMeshPath agentPath = new NavMeshPath();
        agent.CalculatePath(nextWaypoint, agentPath);

        if (agentPath.status == NavMeshPathStatus.PathComplete)
        {
            foreach (Vector3 corner in agentPath.corners)
            {
                //Debug.Log($"=== �ڳ� {corner} �˻� ���� ===");

                foreach (Transform waypoint in allWaypoints)
                {
                    float distance = Vector3.Distance(corner, waypoint.position);
                    //Debug.Log($"�ڳ� {corner}�� ������ {waypoint.name} �� �Ÿ�: {distance}");

                    if (distance < waypointDetectionRadius)
                    {
                        float distanceFromVehicle = Vector3.Distance(transform.position, waypoint.position);
                        //Debug.Log($"������ {waypoint.name}�� �ڳ� {corner}�� ����� (�Ÿ�: {distance})");

                        // �������� �Ÿ��� ����� �ְ� ��ο� ����� �������鸸 �߰�
                        if (distanceFromVehicle > minDistanceFromVehicle)
                        {
                            if (!selectedWaypoints1.Contains(waypoint))
                            {
                                selectedWaypoints1.Add(waypoint);
                                //Debug.Log($"������ {waypoint.name}�� ��ο� ����� ���õ�.");
                            }
                        }
                        else
                        {
                            //Debug.Log($"������ {waypoint.name}�� ������ �ʹ� ����� ���õ��� ����.");
                        }
                    }
                }

                //Debug.Log($"=== �ڳ� {corner} �˻� ���� ===");
            }

            //Debug.Log("���ĵǱ��� ��������:");
            foreach (Transform waypoint in selectedWaypoints1)
            {
                Debug.Log(waypoint.name);
            }
        }

        Left_Waypoints = selectedWaypoints1.Count;

        Debug.Log("Left_Waypoints:" + Left_Waypoints);

        if(Left_Waypoints == 0) { return; }

        // ���� ��ġ���� ���� ������������ ��θ� NavMesh�� ���� ���
        Vector3 nextWaypoint2 = selectedWaypoints1[0].position;

        // ��� ���
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(nextWaypoint2, path);

        if (path.corners.Length > 0)
        {
            // �ڳ� ����Ʈ�� ���� �� ��� ����
            vehiclePathLineRenderer.positionCount = path.corners.Length;
            vehiclePathLineRenderer.SetPositions(path.corners);
        }
        else
        {
            // �ڳ� ����Ʈ�� ���� �� ���� ��ġ�� ���� ������ ��ġ�� �������� ǥ��
            vehiclePathLineRenderer.positionCount = 2;
            vehiclePathLineRenderer.SetPosition(0, transform.position); // ���� ���� ��ġ
            vehiclePathLineRenderer.SetPosition(1, nextWaypoint2);      // ���� ������ ��ġ
        }



    }

    // �������� ���� ���� ��θ� �׸��� �Լ� (��� �� �ִ� �������鸸 ����)
    private void DrawWaypointsPath()
    {

        List<Vector3> waypointsPathPoints = new List<Vector3>();
        Vector3 nextWaypoint = finalDestination.position;
        // ������ NavMeshAgent�� ������ ����� �� �ڳʸ� Ȯ���ϰ�, ��� ��ó�� �������� ����
        NavMeshPath agentPath = new NavMeshPath();
        agent.CalculatePath(nextWaypoint, agentPath);

        if (agentPath.status == NavMeshPathStatus.PathComplete)
        {
            // ���õ� ���������� �̸��� �Ÿ� ������ ���
            Debug.Log("����������:");
            foreach (Transform waypoint in selectedWaypoints1)
            {
                Debug.Log(waypoint.name);
            }

            // ���õ� ���������� ������� �̾ ��� �׸���
            for (int i = 0; i < selectedWaypoints1.Count-1; i++)
            {
                Vector3 adjustedPosition = selectedWaypoints1[i].position;
                adjustedPosition.y += 3.0f; // Y�� ������ �߰�
                waypointsPathPoints.Add(adjustedPosition);
            }


            // ���� ������ �߰�
            Vector3 finalAdjustedPosition = finalDestination.position;
            finalAdjustedPosition.y += 3.0f; // Y�� ������ �߰�
            waypointsPathPoints.Add(finalAdjustedPosition);

            // ������ �� ��� LineRenderer�� ��� ����
            waypointsPathLineRenderer.positionCount = waypointsPathPoints.Count;
            waypointsPathLineRenderer.SetPositions(waypointsPathPoints.ToArray());
        }
    }


}
