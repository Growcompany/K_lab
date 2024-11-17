using System.Collections;
using UnityEngine;
using Barracuda; // Barracuda 사용을 위한 네임스페이스

public class LaneDetection : MonoBehaviour
{
    public Camera vehicleCamera;
    public NNModel yoloModel; // YOLO 모델
    private IWorker worker; // Barracuda worker

    void Start()
    {
        worker = ModelLoader.Load(yoloModel).CreateWorker();
    }

    public Texture2D CaptureImage()
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

    public float GetSteeringAngle()
    {
        Texture2D capturedImage = CaptureImage();
        Tensor inputTensor = new Tensor(capturedImage, 3); // 이미지 텐서 변환
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput(); // YOLO 결과 추출

        // YOLO의 결과를 바탕으로 차선 위치 계산 (단순화된 예시)
        float laneCenterX = CalculateLaneCenter(outputTensor);

        // 차량의 중앙과 차선 중앙의 차이를 바탕으로 조향 각도 계산
        float steeringAngle = (laneCenterX - 640) / 640; // 예시로 단순히 계산

        return steeringAngle;
    }

    private float CalculateLaneCenter(Tensor outputTensor)
    {
        // YOLO 결과를 바탕으로 차선의 중앙을 계산하는 로직을 추가해야 합니다.
        return 640f; // 단순화된 예시 (중앙으로 고정)
    }

    private void OnDestroy()
    {
        worker.Dispose(); // worker 메모리 해제
    }
}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenCvSharp; // OpenCV 사용을 위한 네임스페이스

public class LaneDetection : MonoBehaviour
{
    public Camera captureCamera; // 차량에 부착된 카메라
    public int imageWidth = 1280;
    public int imageHeight = 720;

    public float handleSensitivity = 3.0f; // 핸들 민감도 조정

    private string folderPath;
    //public CarControllerMain carController; // 차량 제어 스크립트 연결
     private float steeringAngle;

    void Start()
    {
        // 폴더 생성
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
            // 이미지를 캡처하고 차선에 따른 핸들 값을 계산
            Texture2D image = CaptureImage();
            steeringAngle = CalculateSteeringAngle(image);

            Debug.Log($"핸들 각도: {steeringAngle}");

            // 차량 핸들 조작 적용
            //carController.SetSteering(steeringAngle);

            yield return new WaitForSeconds(0.5f); // 0.5초마다 이미지 캡처 및 분석
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
        // 1. 이미지 분석을 위해 OpenCV의 Mat 형식으로 변환
        byte[] imageBytes = image.EncodeToPNG();
        Mat matImage = Mat.FromImageData(imageBytes, ImreadModes.Color);

        // 2. 차선 검출 알고리즘 적용 (흑백 변환 및 Canny 엣지 검출 등)
        Mat gray = new Mat();
        Cv2.CvtColor(matImage, gray, ColorConversionCodes.BGR2GRAY);
        Mat edges = new Mat();
        Cv2.Canny(gray, edges, 50, 150);

        // 3. 차선 후보를 추출하기 위한 HoughLinesP 알고리즘
        LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Mathf.PI / 180, 50, 50, 10);

        // 4. 차선 중앙 계산 (차선을 따라 가기 위한 중앙 값 계산)
        float centerX = imageWidth / 2f;
        float laneCenterX = 0;
        int laneCount = 0;

        foreach (var line in lines)
        {
            laneCenterX += (line.P1.X + line.P2.X) / 2; // 차선의 중심 계산
            laneCount++;
        }

        if (laneCount > 0)
        {
            laneCenterX /= laneCount;
        }
        else
        {
            // 차선을 찾지 못한 경우
            return 0; // 핸들 조작 필요 없음
        }

        // 5. 차량의 중앙과 차선 중앙의 차이를 기반으로 핸들 조작 각도 계산
        float offset = laneCenterX - centerX;
        float steeringAngle = handleSensitivity * offset / (imageWidth / 2f) * 7;

        return -steeringAngle;
    }
    public float GetSteeringAngle()
    {
        return steeringAngle;
    }
}
*/