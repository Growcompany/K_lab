using UnityEngine;
using UnityEngine.UI;

public class LaneDectectionUI : MonoBehaviour
{
    public LaneDetection laneDetection; // LaneDetection ��ũ��Ʈ ����
    public RawImage leftImage; // ���� �̹���
    public RawImage rightImage; // ������ �̹���
    public Color normalColor = Color.white; // �⺻ ����
    public Color activeColor = Color.red; // Ȱ��ȭ�� ����

    void Start()
    {
        if (laneDetection == null)
        {
            Debug.LogError("LaneDetection ��ũ��Ʈ�� �������� �ʾҽ��ϴ�!");
        }

        if (leftImage == null || rightImage == null)
        {
            Debug.LogError("UI �̹����� �������� �ʾҽ��ϴ�!");
        }
    }

    void Update()
    {
        if (laneDetection == null || leftImage == null || rightImage == null)
            return;

        // LaneDetection���� steeringAngle �� ��������
        float steeringAngle = laneDetection.GetSteeringAngle();

        // steeringAngle�� ���� �̹��� ���� ������Ʈ
        if (steeringAngle > 0)
        {
            // ���� �̹��� Ȱ��ȭ
            leftImage.color = activeColor;
            rightImage.color = normalColor;
        }
        else if (steeringAngle < 0)
        {
            // ������ �̹��� Ȱ��ȭ
            rightImage.color = activeColor;
            leftImage.color = normalColor;
        }
        else
        {
            // �⺻ ���·� ����
            leftImage.color = normalColor;
            rightImage.color = normalColor;
        }
    }
}
