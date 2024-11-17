using System.Collections;
using UnityEngine;
using Barracuda;

public class TrafficLightDetection : MonoBehaviour
{
    public Camera vehicleCamera;
    public NNModel yoloModel; // YOLO 모델
    private IWorker worker; // Barracuda worker

    void Start()
    {
        worker = ModelLoader.Load(yoloModel).CreateWorker();
    }

    public bool IsRedLight()
    {
        Texture2D capturedImage = CaptureImage();
        Tensor inputTensor = new Tensor(capturedImage, 3); // 이미지 텐서 변환
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput(); // YOLO 결과 추출

        // YOLO 결과를 바탕으로 신호등이 빨간불인지 판단하는 로직 추가
        return IsRedLightDetected(outputTensor);
    }

    private bool IsRedLightDetected(Tensor outputTensor)
    {
        // YOLO 모델의 결과를 분석하여 신호등을 판별하는 로직을 추가해야 합니다.
        return false; // 예시 (빨간불이 아니라고 가정)
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
        worker.Dispose(); // worker 메모리 해제
    }
}
