using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;  // �̱������� ���� ������ ���� ����
    public Camera[] cameras;  // ����� ī�޶� �迭
    private int currentCameraIndex = 0;  // ���� Ȱ��ȭ�� ī�޶� �ε���
    public Image[] uiImages; // UI �̹��� �迭 (�� ī�޶� �����Ǵ� �̹���)

    public Color normalColor = new Color(1f, 1f, 1f, 1f); // �⺻ ���� (���� ����)
    public Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1f); // ��ο� ���� (���� ����)

    void Awake()
    {
        // �̱��� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ��ü ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� ������ ���ο� ��ü �ı�
        }
    }

    void Start()
    {
        // ��� ī�޶� �ʱ�ȭ �� ��Ȱ��ȭ
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // ù ��° ī�޶� Ȱ��ȭ
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // ���� Ű 1~5�� ī�޶� ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchCamera(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchCamera(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchCamera(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SwitchCamera(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // ī�޶� 5���� ��ġ�� ī�޶� 1�� ��ġ�� ���� �� ��ȯ
            cameras[4].transform.position = cameras[0].transform.position;
            SwitchCamera(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) { SwitchCamera(5); }
    }

    public void SwitchCamera(int cameraIndex)
    {
        // ��ȿ�� �ε����� ��� ī�޶� ��ȯ
        if (cameraIndex >= 0 && cameraIndex < cameras.Length)
        {
            cameras[currentCameraIndex].gameObject.SetActive(false); // ���� ī�޶� ��Ȱ��ȭ
            cameras[cameraIndex].gameObject.SetActive(true); // ���ο� ī�޶� Ȱ��ȭ
            currentCameraIndex = cameraIndex; // ���� ī�޶� �ε��� ������Ʈ
        }

        UpdateUIImageColors(cameraIndex); // UI �̹��� ���� ������Ʈ
    }

    void UpdateUIImageColors(int buttonIndex)
    {
        // ��� UI �̹����� �⺻ �������� �ʱ�ȭ
        for (int i = 0; i < uiImages.Length; i++)
        {
            uiImages[i].color = normalColor;
        }

        // Ȱ��ȭ�� ī�޶� �����ϴ� UI �̹��� ���� ����
        if (buttonIndex >= 0 && buttonIndex < uiImages.Length)
        {
            uiImages[buttonIndex].color = darkColor;
        }
    }

    void DisableCameras(int start, int end)
    {
        // Ư�� ������ ī�޶���� ��Ȱ��ȭ
        for (int i = start - 1; i < end; i++)
        {
            if (i >= 0 && i < cameras.Length)
            {
                cameras[i].gameObject.SetActive(false);
            }
        }
    }

    public int GetCurrentCameraIndex()
    {
        return currentCameraIndex; // ���� ī�޶� �ε����� ��ȯ
    }
}
