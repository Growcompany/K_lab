using UnityEngine;
using UnityEngine.UI;

public class LowQualityToggleController : MonoBehaviour
{
    public Toggle objectToggle; // UI Toggle
    public GameObject targetObject; // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ

    private void Start()
    {
        // Toggle�� ���°� ����� �� �̺�Ʈ ����
        objectToggle.onValueChanged.AddListener(delegate { ToggleObject(objectToggle.isOn); });

        // ���� �� �ʱ� ���� ����
        targetObject.SetActive(objectToggle.isOn);
    }

    private void ToggleObject(bool isOn)
    {
        targetObject.SetActive(isOn);
    }
}
