using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenCvSharp; // OpenCV ����� ���� ���ӽ����̽�

public class LaneDetection : MonoBehaviour
{
    public Camera captureCamera; // ������ ������ ī�޶�
    public int imageWidth = 1280;
    public int imageHeight = 720;

    public float handleSensitivity = 3.0f; // �ڵ� �ΰ��� ����

    private string folderPath;
    public CarControllerMain carController; // ���� ���� ��ũ��Ʈ ����

    void Start()
    {
        // ���� ����
        folderPath = Path.Combine(Application.dataPath, "CapturedImages");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        StartCoroutine(CaptureRoutine());
    }

    IEnumerator CaptureRoutine()
    {
        while (true)
        {
            // �̹����� ĸó�ϰ� ������ ���� �ڵ� ���� ���
            Texture2D image = CaptureImage();
            float steeringAngle = CalculateSteeringAngle(image);

            Debug.Log($"�ڵ� ����: {steeringAngle}");

            // ���� �ڵ� ���� ����
            carController.SetSteering(steeringAngle);

            yield return new WaitForSeconds(0.5f); // 0.5�ʸ��� �̹��� ĸó �� �м�
        }
    }

    Texture2D CaptureImage()
    {
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenShot = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new UnityEngine.Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenShot.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        return screenShot;
    }

    float CalculateSteeringAngle(Texture2D image)
    {
        // 1. �̹��� �м��� ���� OpenCV�� Mat �������� ��ȯ
        byte[] imageBytes = image.EncodeToPNG();
        Mat matImage = Mat.FromImageData(imageBytes, ImreadModes.Color);

        // 2. ���� ���� �˰��� ���� (��� ��ȯ �� Canny ���� ���� ��)
        Mat gray = new Mat();
        Cv2.CvtColor(matImage, gray, ColorConversionCodes.BGR2GRAY);
        Mat edges = new Mat();
        Cv2.Canny(gray, edges, 50, 150);

        // 3. ���� �ĺ��� �����ϱ� ���� HoughLinesP �˰���
        LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Mathf.PI / 180, 50, 50, 10);

        // 4. ���� �߾� ��� (������ ���� ���� ���� �߾� �� ���)
        float centerX = imageWidth / 2f;
        float laneCenterX = 0;
        int laneCount = 0;

        foreach (var line in lines)
        {
            laneCenterX += (line.P1.X + line.P2.X) / 2; // ������ �߽� ���
            laneCount++;
        }

        if (laneCount > 0)
        {
            laneCenterX /= laneCount;
        }
        else
        {
            // ������ ã�� ���� ���
            return 0; // �ڵ� ���� �ʿ� ����
        }

        // 5. ������ �߾Ӱ� ���� �߾��� ���̸� ������� �ڵ� ���� ���� ���
        float offset = laneCenterX - centerX;
        float steeringAngle = handleSensitivity * offset / (imageWidth / 2f) * 7;

        return -steeringAngle;
    }

}
