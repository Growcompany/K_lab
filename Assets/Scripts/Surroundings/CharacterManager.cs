using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public ObjectPool objectPool; // Object Pool ����
    public Transform mainCar; // ���� ī Transform
    public float radius = 50f; // ���� ī �ݰ�
    public Transform waypointParent; // Waypoint�� ��� �ִ� �θ� ������Ʈ
    private Transform[] waypoints; // ��� Waypoint �迭

    void Start()
    {
        // WaypointParent �Ʒ� �ڽ� ������Ʈ�� �ڵ����� �迭�� �߰�
        waypoints = new Transform[waypointParent.childCount];
        for (int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i); // �ڽ� Waypoint ��������
        }
    }

    void Update()
    {
        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(mainCar.position, waypoint.position);
            if (distance <= radius)
            {
                // �ݰ� ���� ���� Waypoint��� ĳ���� Ȱ��ȭ
                ActivateCharacterAtWaypoint(waypoint);
            }
        }
    }

    void ActivateCharacterAtWaypoint(Transform waypoint)
    {
        GameObject character = objectPool.GetObject(); // Ǯ���� ĳ���� ��������
        character.transform.position = waypoint.position; // Waypoint ��ġ�� ��ġ
        character.GetComponent<CharacterAI>().StartMoving(); // �̵� ����
    }
}
