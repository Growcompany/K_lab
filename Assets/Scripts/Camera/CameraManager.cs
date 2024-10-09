using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;  // �̱��� ����- ������ ����
    public Camera[] cameras;
    private int currentCameraIndex = 0;
    public Image[] uiImages; // UI �̹��� �迭 (ī�޶� �����ϴ� �̹�����)

    public Color normalColor = new Color(1f, 1f, 1f, 1f); // ���� ���� (���� ����)
    public Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1f); // ��ο� ���� (���� ����)

    void Awake()
    {
        // �̱��� �������� ī�޶� �Ŵ��� ���� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ�ÿ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // ���� 1 ~ 5 Ű�� ī�޶� ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchCamera(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchCamera(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchCamera(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SwitchCamera(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            cameras[4].transform.position = cameras[0].transform.position;
            SwitchCamera(4);

        }
    }

    public void SwitchCamera(int cameraIndex)
    {
        if (cameraIndex >= 0 && cameraIndex < cameras.Length)
        {
            cameras[currentCameraIndex].gameObject.SetActive(false);
            cameras[cameraIndex].gameObject.SetActive(true);
            currentCameraIndex = cameraIndex;
        }

        UpdateUIImageColors(cameraIndex);
    }
    void UpdateUIImageColors(int buttonIndex)
    {
        // ��� �̹����� ������ ���� ������ ������ ��
        for (int i = 0; i < uiImages.Length; i++)
        {
            uiImages[i].color = normalColor;
        }

        // Ȱ��ȭ�� ī�޶� �ش��ϴ� �̹��� ���� ��Ӱ� ����
        uiImages[buttonIndex].color = darkColor;
    }
    public int GetCurrentCameraIndex()
    {
        return currentCameraIndex; // ���� ī�޶� �ε��� ��ȯ
    }
}
