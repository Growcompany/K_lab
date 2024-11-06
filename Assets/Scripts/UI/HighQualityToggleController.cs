using UnityEngine;
using UnityEngine.UI;

public class LowQualityToggleController : MonoBehaviour
{
    public Toggle objectToggle; // UI Toggle
    public GameObject targetObject; // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ

    private void Start()
    {
        // targetObject�� �Ҵ���� ���� ��� �ʱ�ȭ �ߴ�
        if (targetObject == null)
        {
            Debug.LogWarning("Target Object is not assigned, skipping initialization.");
            return;
        }

        // Toggle�� ���°� ����� �� �̺�Ʈ ����
        objectToggle.onValueChanged.AddListener(delegate { ToggleObject(objectToggle.isOn); });

        // ���� �� �ʱ� ���� ����
        targetObject.SetActive(objectToggle.isOn);
    }

    private void ToggleObject(bool isOn)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(isOn);
        }
    }
}
