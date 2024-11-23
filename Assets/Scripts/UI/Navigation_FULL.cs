using UnityEngine;
using UnityEngine.UI;

public class Navigation_FULL : MonoBehaviour
{
    public Toggle controlToggle; // UI ���
    public Transform destination1; // ù ��° ������ ��ġ
    public Transform destination2; // �� ��° ������ ��ġ
    public Transform destination3; // �� ��° ������
    public Transform destination4; // �� ��° ������ ��ġ
    public Transform destination5; // �� ��° ������ ��ġ
    public VehicleNavigation vehicleNavigation;

    private void Start()
    {
        if (controlToggle != null)
        {
            // �ʱ� ���¸� ��ۿ� ���߰� �̺�Ʈ�� �߰�
            gameObject.SetActive(controlToggle.isOn);
            controlToggle.onValueChanged.AddListener(SetVisibility);
        }
    }

    private void SetVisibility(bool isVisible)
    {
        // ��� ���¿� ���� ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
        gameObject.SetActive(isVisible);
    }

    public void SetDestination1()
    {
        vehicleNavigation.SetDestination(destination1);
    }

    public void SetDestination2()
    {
        vehicleNavigation.SetDestination(destination2);
    }

    public void SetDestination3()
    {
        vehicleNavigation.SetDestination(destination3);
    }
    public void SetDestination4()
    {
        vehicleNavigation.SetDestination(destination4);
    }
    public void SetDestination5()
    {
        vehicleNavigation.SetDestination(destination5);
    }
}
