using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public Camera captureCamera; // ĸó�� ī�޶�
    public int imageWidth = 1280;
    public int imageHeight = 720;
    public float captureInterval = 0.5f; // 0.5�ʸ��� �� �徿 ����

    private string folderPath;

    void Start()
    {
        // 1. ���� ���� (���� ������Ʈ ��� �Ʒ��� "CapturedImages" ���� ����)
        folderPath = Path.Combine(Application.dataPath, "CapturedImages");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"������ �����Ǿ����ϴ�: {folderPath}");
        }

        // 2. ĸó �ڷ�ƾ ����
        StartCoroutine(CaptureRoutine());
    }

    IEnumerator CaptureRoutine()
    {
        while (true)
        {
            CaptureImage();
            yield return new WaitForSeconds(captureInterval);
        }
    }

    void CaptureImage()
    {
        // 1. RenderTexture ���� �� ī�޶� �Ҵ�
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        captureCamera.targetTexture = renderTexture;

        // 2. ī�޶� ������
        captureCamera.Render();

        // 3. RenderTexture���� Texture2D�� ��ȯ
        RenderTexture.active = renderTexture;
        Texture2D screenShot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenShot.Apply();

        // 4. RenderTexture �� ī�޶� ���� ����
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // 5. ���� �̸� ���� (Ÿ�ӽ����� ���)
        string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
        string filePath = Path.Combine(folderPath, $"Capture_{timeStamp}.png");

        // 6. Texture2D �����͸� PNG ���Ϸ� ��ȯ �� ����
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"�̹����� {filePath}�� ����Ǿ����ϴ�.");
    }
}
