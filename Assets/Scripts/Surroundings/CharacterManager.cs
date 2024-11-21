using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public ObjectPool objectPool; // Object Pool ����
    public Transform mainCar; // MainCar Transform
    public float radius = 50f; // MainCar �ݰ�
    public GameObject[] waypointParents; // ��� WaypointParent �׷�
    public int maxCharactersPerParent = 10; // �� Parent �׷�� �ִ� ĳ���� ��

    private Dictionary<GameObject, List<GameObject>> activeCharacters = new Dictionary<GameObject, List<GameObject>>();

    void Start()
    {
        // �� WaypointParent �׷� �ʱ�ȭ
        foreach (var parent in waypointParents)
        {
            activeCharacters[parent] = new List<GameObject>();
        }
    }

    void Update()
    {
        foreach (var parent in waypointParents)
        {
            List<Transform> activeWaypoints = GetActiveWaypoints(parent);

            // �̹� ������ ĳ������ ���� �ʰ����� �ʵ��� ����
            while (activeCharacters[parent].Count < maxCharactersPerParent && activeWaypoints.Count > 0)
            {
                Transform waypoint = activeWaypoints[Random.Range(0, activeWaypoints.Count)];
                CreateCharacterAtWaypoint(parent, waypoint);
                activeWaypoints.Remove(waypoint); // �ߺ� ���� ����
            }

            // �ݰ濡�� ��� ĳ���� ��Ȱ��ȭ
            DisableCharactersOutOfRange(parent, activeWaypoints);
        }
    }

    private List<Transform> GetActiveWaypoints(GameObject waypointParent)
    {
        List<Transform> activeWaypoints = new List<Transform>();
        Transform parentTransform = waypointParent.transform;

        foreach (Transform waypoint in parentTransform)
        {
            float distance = Vector3.Distance(mainCar.position, waypoint.position);
            if (distance <= radius) // MainCar �ݰ� �� Ȯ��
            {
                activeWaypoints.Add(waypoint);
            }
        }

        return activeWaypoints;
    }

    private void CreateCharacterAtWaypoint(GameObject waypointParent, Transform waypoint)
    {
        GameObject character = objectPool.GetObject();
        character.transform.position = waypoint.position;
        CharacterAI ai = character.GetComponent<CharacterAI>();
        ai.SetWaypointsParent(waypointParent); // �ش� Parent ����
        ai.StartMoving();

        // Ȱ�� ĳ���� ����Ʈ�� �߰�
        activeCharacters[waypointParent].Add(character);
    }

    private void DisableCharactersOutOfRange(GameObject waypointParent, List<Transform> activeWaypoints)
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject character in activeCharacters[waypointParent])
        {
            bool stillActive = false;

            foreach (Transform waypoint in activeWaypoints)
            {
                if (Vector3.Distance(character.transform.position, waypoint.position) < radius)
                {
                    stillActive = true;
                    break;
                }
            }

            if (!stillActive)
            {
                character.SetActive(false); // ĳ���� ��Ȱ��ȭ
                toRemove.Add(character);
            }
        }

        // ����Ʈ���� ����
        foreach (GameObject character in toRemove)
        {
            activeCharacters[waypointParent].Remove(character);
        }
    }
}
