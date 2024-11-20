using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public ObjectPool objectPool; // Object Pool ����
    public Transform mainCar; // ���� ī Transform
    public float radius = 50f; // ���� ī �ݰ�
    public GameObject waypointParent; // Waypoint Parent (GameObject)
    public int maxCharacters = 3; // ������ ĳ������ �ִ� ��

    private Transform[] waypoints; // Waypoints �迭
    private HashSet<Transform> usedWaypoints = new HashSet<Transform>(); // ���� Waypoints

    void Start()
    {
        // Waypoint Parent���� ��� �ڽ� Transform ��������
        Transform parentTransform = waypointParent.transform;
        waypoints = new Transform[parentTransform.childCount];
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            waypoints[i] = parentTransform.GetChild(i);
        }
    }

    void Update()
    {
        List<Transform> activeWaypoints = GetActiveWaypoints();

        // ���� Ȱ��ȭ�� Waypoint���� ĳ���� ����
        int charactersToSpawn = Mathf.Min(maxCharacters, activeWaypoints.Count);
        for (int i = 0; i < charactersToSpawn; i++)
        {
            Transform waypoint = activeWaypoints[Random.Range(0, activeWaypoints.Count)];
            if (!usedWaypoints.Contains(waypoint))
            {
                ActivateCharacterAtWaypoint(waypoint);
                usedWaypoints.Add(waypoint);
            }
        }
    }

    private List<Transform> GetActiveWaypoints()
    {
        List<Transform> activeWaypoints = new List<Transform>();
        foreach (Transform waypoint in waypoints)
        {
            if (Vector3.Distance(mainCar.position, waypoint.position) <= radius)
            {
                activeWaypoints.Add(waypoint);
            }
        }
        return activeWaypoints;
    }

    private void ActivateCharacterAtWaypoint(Transform waypoint)
    {
        GameObject character = objectPool.GetObject();
        character.transform.position = waypoint.position;
        CharacterAI ai = character.GetComponent<CharacterAI>();
        ai.SetWaypointsParent(waypointParent); // Waypoint Parent ����
        ai.StartMoving();
    }
}
