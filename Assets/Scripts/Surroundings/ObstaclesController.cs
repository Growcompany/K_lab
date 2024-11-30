using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject[] obstacles; // ������ ��ֹ� ������Ʈ �迭
    public float redLightDuration = 10f; // ���� ������ ���� �ð�
    public float greenLightDuration = 17f; // ���� �ʷϺ� ���� �ð�
    public float yellowLightDuration = 3f; // ���� ����� ���� �ð�

    private float timer = 0f; // Ÿ�̸�
    private int state = 0; // 0 = ���� ������, 1 = ���� �ʷϺ�, 2 = ���� �����

    void Start()
    {
        SetObstacleState(true); // �ʱ� ����: ��ֹ� Ȱ��ȭ
    }

    void Update()
    {
        timer += Time.deltaTime;

        //switch (state)
        //{
        //    case 0: // ���� ������
        //        if (timer > redLightDuration)
        //        {
        //            timer = 0f;
        //            state = 1; // ���� ���·� ��ȯ (�ʷϺ�)
        //            SetObstacleState(false); // ��ֹ� ��Ȱ��ȭ
        //        }
        //        break;

        //    case 1: // ���� �ʷϺ�
        //        if (timer > greenLightDuration)
        //        {
        //            timer = 0f;
        //            state = 2; // ���� ���·� ��ȯ (�����)
        //        }
        //        break;

        //    case 2: // ���� �����
        //        if (timer > yellowLightDuration)
        //        {
        //            timer = 0f;
        //            state = 0; // �ʱ� ���·� ���ư� (������)
        //            SetObstacleState(true); // ��ֹ� Ȱ��ȭ
        //        }
        //        break;
        //}

        switch (state)
        {
            case 0: // ���� ������, ������ �ʷϺ�
                if (timer > 10f) // 10�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 1; // ���� ���·� ����
                    foreach (GameObject group in obstacles)
                    {
                        SetObstacleState(false); // ���� �ʷϺҷ� ��ȯ
                    }
                }
                break;
            case 1: // ���� �ʷϺ�, ������ ������
                if (timer > 17f) // 17�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 2; // ���� ���·� ����
                    foreach (GameObject group in obstacles)
                    {
                        SetObstacleState(false); // ���� ����ҷ� ��ȯ
                    }
                }
                break;
            case 2: // ���� �����, ������ ������
                if (timer > 3f) // 3�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 0; // �ʱ� ���·� ���ư�
                    foreach (GameObject group in obstacles)
                    {
                        SetObstacleState(true); // ���� �����ҷ� ��ȯ
                    }
                }
                break;
        }
    }

    void SetObstacleState(bool active)
    {
        // ��ֹ� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null) // ��ֹ��� �����ϴ� ��츸 ó��
            {
                obstacle.SetActive(active);
            }
        }
    }
}
