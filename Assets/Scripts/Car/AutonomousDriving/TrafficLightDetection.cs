using System.Collections;
using UnityEngine;
using Barracuda;

public class TrafficLightDetection : MonoBehaviour
{
    public Camera vehicleCamera;
    public NNModel yoloModel; // YOLO ��
    private IWorker worker; // Barracuda worker

    void Start()
    {
        worker = ModelLoader.Load(yoloModel).CreateWorker();
    }

    public bool IsRedLight()
    {
        Texture2D capturedImage = CaptureImage();
        Tensor inputTensor = new Tensor(capturedImage, 3); // �̹��� �ټ� ��ȯ
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput(); // YOLO ��� ����

        // YOLO ����� �������� ��ȣ���� ���������� �Ǵ��ϴ� ���� �߰�
        return IsRedLightDetected(outputTensor);
    }

    private bool IsRedLightDetected(Tensor outputTensor)
    {
        // YOLO ���� ����� �м��Ͽ� ��ȣ���� �Ǻ��ϴ� ������ �߰��ؾ� �մϴ�.
        return false; // ���� (�������� �ƴ϶�� ����)
    }

    private Texture2D CaptureImage()
    {
        RenderTexture renderTexture = new RenderTexture(1280, 720, 24);
        vehicleCamera.targetTexture = renderTexture;
        vehicleCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D capturedImage = new Texture2D(1280, 720, TextureFormat.RGB24, false);
        capturedImage.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
        capturedImage.Apply();

        vehicleCamera.targetTexture = null;
        RenderTexture.active = null;

        return capturedImage;
    }

    private void OnDestroy()
    {
        worker.Dispose(); // worker �޸� ����
    }
}
