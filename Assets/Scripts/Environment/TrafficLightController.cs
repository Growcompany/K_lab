using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public GameObject[] trafficLightGroups; // ��ȣ�� �׷� (�θ� ������Ʈ��)
    public Transform vehicleTransform; // ������ Transform

    private float timer = 0f;
    private int state = 0; // 0 = ���� ������, 1 = ���� �ʷϺ�, 2 = ���� �����

    void Start()
    {
        // �� ��ȣ�� �׷��� �ʱ� ���� ����
        foreach (GameObject group in trafficLightGroups)
        {
            SetTrafficLightState(group, 0); // �ʱ� ���¸� ���� �����ҷ� ����
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case 0: // ���� ������, ������ �ʷϺ�
                if (timer > 10f) // 10�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 1; // ���� ���·� ����
                    foreach (GameObject group in trafficLightGroups)
                    {
                        SetTrafficLightState(group, 1); // ���� �ʷϺҷ� ��ȯ
                    }
                }
                break;
            case 1: // ���� �ʷϺ�, ������ ������
                if (timer > 17f) // 17�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 2; // ���� ���·� ����
                    foreach (GameObject group in trafficLightGroups)
                    {
                        SetTrafficLightState(group, 2); // ���� ����ҷ� ��ȯ
                    }
                }
                break;
            case 2: // ���� �����, ������ ������
                if (timer > 3f) // 3�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 0; // �ʱ� ���·� ���ư�
                    foreach (GameObject group in trafficLightGroups)
                    {
                        SetTrafficLightState(group, 0); // ���� �����ҷ� ��ȯ
                    }
                }
                break;
        }
    }

    void SetTrafficLightState(GameObject group, int newState)
    {
        // �׷� �� �ڽ� ������Ʈ ã�� (���ο� �̸� ����)
        Transform vehicleRedLight = group.transform.Find("Vehicle_Red");
        Transform vehicleYellowLight = group.transform.Find("Vehicle_Yellow");
        Transform vehicleGreenLight = group.transform.Find("Vehicle_Green");
        Transform pedestrianRedLight = group.transform.Find("Crosswalk_Red");
        Transform pedestrianGreenLight = group.transform.Find("Crosswalk_Green");

        // �׷� �� AudioSource ������Ʈ ��������
        AudioSource audioSource = group.GetComponent<AudioSource>();

        // ��� ��ȣ�� ��Ȱ��ȭ
        vehicleRedLight.gameObject.SetActive(false);
        vehicleYellowLight.gameObject.SetActive(false);
        vehicleGreenLight.gameObject.SetActive(false);
        pedestrianRedLight.gameObject.SetActive(true); // �⺻ ������ ������ �ѱ�
        pedestrianGreenLight.gameObject.SetActive(false);

        // ���ο� ���¿� ���� ��ȣ�� Ȱ��ȭ �� �Ҹ� ����
        switch (newState)
        {
            case 0: // ���� ������, ������ �ʷϺ�
                vehicleRedLight.gameObject.SetActive(true);
                pedestrianGreenLight.gameObject.SetActive(true);
                pedestrianRedLight.gameObject.SetActive(false);
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play(); // ������ �ʷϺ��� �� �Ҹ� ���
                }
                break;
            case 1: // ���� �ʷϺ�, ������ ������
                vehicleGreenLight.gameObject.SetActive(true);
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop(); // �Ҹ� ����
                }
                break;
            case 2: // ���� �����, ������ ������
                vehicleYellowLight.gameObject.SetActive(true);
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop(); // �Ҹ� ����
                }
                break;
        }
    }


}
