using System.Collections;
using UnityEngine;
using OpenCvSharp; // OpenCV ���ӽ����̽� ���

public class LaneDetection : MonoBehaviour
{
    public Camera captureCamera; // ������ ������ ī�޶�
    public RenderTexture renderTexture; // ī�޶� ����� RenderTexture
    public float handleSensitivity = 0.5f; // �ڵ� �ΰ��� ����

    private float steeringAngle;

    void Start()
    {
        if (captureCamera == null)
        {
            Debug.LogError("CaptureCamera�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture�� �������� �ʾҽ��ϴ�!");
            return;
        }

        StartCoroutine(CaptureRoutine());
    }

    // �ֱ������� �̹����� ĸó�ϰ� ������ ���� �ڵ� ���� ����ϴ� �ڷ�ƾ
    IEnumerator CaptureRoutine()
    {
        while (true)
        {
            // RenderTexture���� �̹����� �о��
            Texture2D image = ReadRenderTexture(renderTexture);

            // �̹����� ó���Ͽ� ������ ���� ���� ���� ���
            steeringAngle = CalculateSteeringAngle(image);

            yield return new WaitForSeconds(0.05f); // 0.05�ʸ��� ������ ó��
        }
    }

    // RenderTexture���� Texture2D�� �����ϴ� �Լ�
    Texture2D ReadRenderTexture(RenderTexture rt)
    {
        // RenderTexture Ȱ��ȭ
        RenderTexture.active = rt;

        // RenderTexture �����͸� Texture2D�� ����
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = null;

        return tex;
    }

    // ���� ������ ���� ���Ⱒ ��� �Լ�
    float CalculateSteeringAngle(Texture2D image)
    {
        if (image == null)
        {
            Debug.LogWarning("�̹����� ��� �ֽ��ϴ�!");
            return 0;
        }

        // OpenCV�� ����� �̹��� �м�
        byte[] imageBytes = image.EncodeToPNG();
        Mat matImage = Mat.FromImageData(imageBytes, ImreadModes.Color);

        // ��� ��ȯ
        Mat gray = new Mat();
        Cv2.CvtColor(matImage, gray, ColorConversionCodes.BGR2GRAY);

        // ���� ����
        Mat edges = new Mat();
        Cv2.Canny(gray, edges, 50, 150);

        // ���� ����: HoughLinesP�� ����� ������ ����
        LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Mathf.PI / 180, 50, 50, 10);

        // ���� �߾� ���
        float centerX = renderTexture.width / 2f;
        float laneCenterX = 0;
        int laneCount = 0;

        foreach (var line in lines)
        {
            laneCenterX += (line.P1.X + line.P2.X) / 2;
            laneCount++;
        }

        if (laneCount > 0)
        {
            laneCenterX /= laneCount;
        }
        else
        {
            Debug.LogWarning("������ ã�� ���߽��ϴ�!");
            return 0;
        }

        // ���� �߽ɰ� ���� �߽��� ���̸� ������� �ڵ� ���� ���� ���
        float offset = laneCenterX - centerX;
        float normalizedOffset = handleSensitivity * offset / (renderTexture.width / 2f);

        // ���Ⱒ ��� �� ���� ����
        float calculatedSteering = -handleSensitivity * normalizedOffset;
        calculatedSteering = Mathf.Clamp(calculatedSteering, -1f, 1f);

        return calculatedSteering;

        //return -steeringAngle;
    }

    // �ܺο��� ���Ⱒ�� ��ȸ�� �� �ִ� �Լ�
    public float GetSteeringAngle()
    {
        Debug.Log($"Steering Angle: {steeringAngle}");
        return steeringAngle/2;
    }
}

/*
using System.Collections;
using UnityEngine;
using OpenCvSharp;

public class LaneDetection : MonoBehaviour
{
    public Camera captureCamera; // ������ ������ ī�޶�
    public RenderTexture renderTexture; // ī�޶� ����� RenderTexture
    public float handleSensitivity = 0.5f; // �ڵ� �ΰ��� ����

    private PIDController pidController; // PID �����
    private float steeringAngle;

    void Start()
    {
        if (captureCamera == null)
        {
            Debug.LogError("CaptureCamera�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // PIDController �ʱ�ȭ (PI ����: kd=0)
        pidController = new PIDController(1.0f, 0.1f, 0.0f, -1.0f, 1.0f);

        StartCoroutine(CaptureRoutine());
    }

    IEnumerator CaptureRoutine()
    {
        while (true)
        {
            // RenderTexture���� �̹����� �о��
            Texture2D image = ReadRenderTexture(renderTexture);

            // ���� �߽����κ����� ������ ���
            float laneOffset = CalculateLaneOffset(image);

            // PID ������ ���Ⱒ ���
            steeringAngle = pidController.Update(laneOffset, 0.1f); // deltaTime: 0.1��

            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� ó��
        }
    }


    Texture2D ReadRenderTexture(RenderTexture rt)
    {
        // RenderTexture Ȱ��ȭ
        RenderTexture.active = rt;

            // RenderTexture �����͸� Texture2D�� ����
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new UnityEngine.Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            RenderTexture.active = null;

            return tex;
        }

    float CalculateLaneOffset(Texture2D image)
    {
        if (image == null)
        {
            Debug.LogWarning("�̹����� ��� �ֽ��ϴ�!");
            return 0;
        }

        byte[] imageBytes = image.EncodeToPNG();
        Mat matImage = Mat.FromImageData(imageBytes, ImreadModes.Color);

        Mat gray = new Mat();
        Cv2.CvtColor(matImage, gray, ColorConversionCodes.BGR2GRAY);

        Mat edges = new Mat();
        Cv2.Canny(gray, edges, 50, 150);

        LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Mathf.PI / 180, 50, 50, 10);

        float centerX = renderTexture.width / 2f;
        float laneCenterX = 0;
        int laneCount = 0;

        foreach (var line in lines)
        {
            laneCenterX += (line.P1.X + line.P2.X) / 2;
            laneCount++;
        }

        if (laneCount > 0)
        {
            laneCenterX /= laneCount;
        }
        else
        {
            Debug.LogWarning("������ ã�� ���߽��ϴ�!");
            return 0;
        }

        return (laneCenterX - centerX) / (renderTexture.width / 2f);
    }

    public float GetSteeringAngle()
    {
        Debug.Log($"Steering Angle: {steeringAngle}");
        return steeringAngle;
    }
}
*/
