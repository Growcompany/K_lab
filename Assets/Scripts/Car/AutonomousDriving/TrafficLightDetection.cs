using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UI = UnityEngine.UI;

public class TrafficLightDetection : MonoBehaviour
{

    public NNModel _model;
    public UI.RawImage _imageView;
    public Canvas canvas; // UI�� ǥ���� Canvas
    public Camera CaptureCamera; // Unity ī�޶�
    public RenderTexture renderTexture; // Unity ī�޶� ��¿� RenderTexture
    private List<GameObject> detectionBoxes = new List<GameObject>(); // ������ �ڽ� ����


    private int _resizeLength = 640; // ��������� ���簢���� �� ���� ����
                                     // �� ����
                                     // model.Metadata["names"]���� ������ ���� ����Ǿ� ������, JSON ���ڿ��� ��ϵǾ� �־� ǥ�� ������� �Ľ��� �� �����Ƿ� ������ ������.
    private readonly string[] _labels = {
        "car", "light", "null", "person"};

    // Start is called before the first frame update
    void Start()
    {
        // ONNX �� �ε� �� ��Ŀ ����
        var model = ModelLoader.Load(_model);
        var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);


        // Unity ī�޶� RenderTexture ����
        CaptureCamera.targetTexture = renderTexture;
        _imageView.texture = renderTexture; // UI���� �ǽð� ī�޶� �並 ǥ��

        // ������Ʈ �ڷ�ƾ ����
        StartCoroutine(UpdateDetection());


        IEnumerator UpdateDetection()
        {
            while (true)
            {

                // RenderTexture�� Texture2D�� ��ȯ
                Texture2D capturedImage = CaptureCameraImage();

                if (capturedImage == null)
                {
                    Debug.LogError("Captured image is null. Check if RenderTexture is properly assigned.");
                    yield break;
                }

                // �̹����� ONNX �𵨿� �Է�
                var texture = ResizedTexture(capturedImage, _resizeLength, _resizeLength);
                Tensor inputTensor = new Tensor(texture, channels: 3);


                // �߷� ����
                worker.Execute(inputTensor);

                // ��� �м�
                Tensor output0 = worker.PeekOutput("output0");
                List<DetectionResult> ditects = ParseOutputs(output0, 0.3f, 0.75f);

                inputTensor.Dispose();
                output0.Dispose();

                // ��� �׸���
                // ��ҵ� �̹����� �м��ϰ� �����Ƿ�, ����� ���� ũ��� ��ȯ
                float scaleX = capturedImage.width / (float)_resizeLength;
                float scaleY = capturedImage.height / (float)_resizeLength;

                // UI ������Ʈ
                int activeCameraIndex = CameraManager.instance.GetCurrentCameraIndex();
                if (activeCameraIndex == 5) { AddBoxOutline(ditects, scaleX, scaleY); };
                AddBoxOutline_MiniCam(ditects, scaleX, scaleY, capturedImage);

                // ���� ������� 0.5�� ���
                yield return new WaitForSeconds(0.1f);
            }
        }

        //worker.Dispose();

    }

    private Texture2D CaptureCameraImage()
    {
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        return texture;
    }

    private void AddBoxOutline(List<DetectionResult> detections, float scaleX, float scaleY)
    {
        // ���� �������� �� ����
        foreach (GameObject line in detectionBoxes)
        {
            Destroy(line);
        }
        detectionBoxes.Clear(); // ����Ʈ �ʱ�ȭ

        // ĵ���� ũ�� Ȯ��
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // ȭ�� ���� ���� (640x360 ���� ����)
        float referenceWidth = 640f;
        float referenceHeight = 360f;
        float scaleFactorX = canvasWidth / referenceWidth;
        float scaleFactorY = canvasHeight / referenceHeight;

        foreach (DetectionResult detection in detections)
        {
            // �ڽ� ��ǥ ��� (���� �ݿ�)
            float x1 = detection.x1 * scaleX * scaleFactorX;
            float y1 = detection.y1 * scaleY * scaleFactorY;
            float x2 = detection.x2 * scaleX * scaleFactorX;
            float y2 = detection.y2 * scaleY * scaleFactorY;

            // UI ��ǥ �ݿ� (Y�� ����)
            float uiY1 = canvasHeight - y1;
            float uiY2 = canvasHeight - y2;

            // Ŭ���� ID�� ���� ���� ����
            Color lineColor = Color.red; // �⺻ ����: ����
            if (detection.classId == 0) // 0�� Ŭ����: "car"
            {
                lineColor = Color.blue; 
            }
            else if (detection.classId == 1) // 1�� Ŭ����: "light"
            {
                lineColor = Color.green; 
            }
            else if (detection.classId == 2) // 2�� Ŭ����: "person"
            {
                lineColor = Color.red;
            }
            else
            {
                continue;
            }

            // 4���� ���� �����Ͽ� �ڽ� ��輱�� �׸�
            CreateLine(new Vector2(x1, uiY1), new Vector2(x2, uiY1), lineColor); // ���
            CreateLine(new Vector2(x2, uiY1), new Vector2(x2, uiY2), lineColor); // ����
            CreateLine(new Vector2(x2, uiY2), new Vector2(x1, uiY2), lineColor); // �ϴ�
            CreateLine(new Vector2(x1, uiY2), new Vector2(x1, uiY1), lineColor); // ����
        }
    }


    private void CreateLine(Vector2 start, Vector2 end, Color color)
    {
        // ���� �׸� GameObject ����
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.SetParent(canvas.transform, false);

        // Image ������Ʈ �߰�
        var image = lineObject.AddComponent<UI.Image>();
        image.color = color; // �� ���� ����

        // RectTransform ����
        RectTransform rectTransform = lineObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0); // Canvas ���� �ϴ� ����
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0.5f); // ���� �߽��� Y�� �߰����� ����

        // ���� ���̿� �β� ���
        float length = Vector2.Distance(start, end);
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

        rectTransform.sizeDelta = new Vector2(length, 2f); // ���� ���̿� �β� (�β� 2)
        rectTransform.anchoredPosition = start; // ������ ��ġ
        rectTransform.rotation = Quaternion.Euler(0, 0, angle); // ȸ�� ����

        // ������ ���� ����Ʈ�� �߰�
        detectionBoxes.Add(lineObject);
    }

    private void AddBoxOutline_MiniCam(List<DetectionResult> detections, float scaleX, float scaleY, Texture2D capturedImage)
    {
        var image = ResizedTexture(capturedImage, capturedImage.width, capturedImage.height);
        // ������ Ŭ������ ������ ������ �ǵ��� ����
        Dictionary<int, Color> colorMap = new Dictionary<int, Color>();

        foreach (DetectionResult ditect in detections)
        {
            // �м� ��� ǥ��
            Debug.Log($"{_labels[ditect.classId]}: {ditect.score:0.00}");

            // �ڽ�ó�� ǥ��
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            if (colorMap.ContainsKey(ditect.classId))
            {
                color = colorMap[ditect.classId];
            }
            else
            {
                colorMap.Add(ditect.classId, color);
            }

            // ��ǥ ������ ����
            int x1 = (int)(ditect.x1 * scaleX);
            int x2 = (int)(ditect.x2 * scaleX);
            int y1 = (int)(ditect.y1 * scaleY);
            int y2 = (int)(ditect.y2 * scaleY);

            // �簢���� ���, �ϴ�, ����, ���� �׸���
            for (int x = x1; x <= x2; x++)
            {
                // ���
                image.SetPixel(x, capturedImage.height - y1, color);
                // �ϴ�
                image.SetPixel(x, capturedImage.height - y2, color);
            }
            for (int y = y1; y <= y2; y++)
            {
                // ����
                image.SetPixel(x1, capturedImage.height - y, color);
                // ����
                image.SetPixel(x2, capturedImage.height - y, color);
            }

        }
        image.Apply();

        _imageView.texture = image;
    }

    // �� ȭ�鿡 ��� ����� ������
    private void AddLabelToDetection(DetectionResult ditect, float scaleX, float scaleY, Texture2D capturedImage)
    {
        // �ڽ� ��ǥ ��� (Texture2D ��ǥ���� Canvas ��ǥ�� ��ȯ)
        float x1 = ditect.x1 * scaleX;
        float y1 = ditect.y1 * scaleY;
        float x2 = ditect.x2 * scaleX;
        float y2 = ditect.y2 * scaleY;

        // �ڽ� �߽� ��ǥ ���
        float centerX = (x1 + x2) / 2;
        float centerY = (y1 + y2) / 2;

        // ��ǥ ���� ���� (Texture2D�� �»�� ����, Canvas�� ���ϴ� ����)
        centerY = capturedImage.height - centerY;

        // �� GameObject ����
        GameObject labelObject = new GameObject("DetectionLabel");
        labelObject.transform.SetParent(canvas.transform);

        // Text ������Ʈ �߰�
        UI.Text label = labelObject.AddComponent<UI.Text>();
        label.text = $"{_labels[ditect.classId]}: {ditect.score:0.00}";
        label.fontSize = 14;
        label.color = Color.white; // �� ����
        label.alignment = TextAnchor.MiddleCenter;

        // RectTransform ����
        RectTransform rectTransform = label.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(150, 30); // �ؽ�Ʈ �ڽ� ũ��
        rectTransform.anchorMin = new Vector2(0, 1); // Canvas�� ��ǥ��� ���߱�
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(centerX, centerY); // �ڽ� �߽ɿ� ��ġ ����
    }


    private List<DetectionResult> ParseOutputs(Tensor output0, float threshold, float iouThres)
    {
        // ���� ����� �� ��
        int outputWidth = output0.shape.width;

        // ���� ����� ä���� �ĺ�
        List<DetectionResult> candidateDitects = new List<DetectionResult>();
        // ����� ���� ���
        List<DetectionResult> ditects = new List<DetectionResult>();

        for (int i = 0; i < outputWidth; i++)
        {
            // ���� ��� �м�
            var result = new DetectionResult(output0, i);
            // ������ ���� �� �̸��̸� ����
            if (result.score < threshold)
            {
                continue;
            }
            // �ĺ��� �߰�
            candidateDitects.Add(result);
        }

        // NonMaxSuppression ó��
        // ��ģ �簢�� �� �ִ� ������ ���� ���� ä��
        while (candidateDitects.Count > 0)
        {
            int idx = 0;
            float maxScore = 0.0f;
            for (int i = 0; i < candidateDitects.Count; i++)
            {
                if (candidateDitects[i].score > maxScore)
                {
                    idx = i;
                    maxScore = candidateDitects[i].score;
                }
            }

            // ������ ���� ���� ����� ��������, ����Ʈ���� ����
            var cand = candidateDitects[idx];
            candidateDitects.RemoveAt(idx);

            // ä���� ����� �߰�
            ditects.Add(cand);

            List<int> deletes = new List<int>();
            for (int i = 0; i < candidateDitects.Count; i++)
            {
                // IOU Ȯ��
                float iou = Iou(cand, candidateDitects[i]);
                if (iou >= iouThres)
                {
                    deletes.Add(i);
                }
            }
            for (int i = deletes.Count - 1; i >= 0; i--)
            {
                candidateDitects.RemoveAt(deletes[i]);
            }

        }

        return ditects;

    }


    // ��ü�� ��ħ ���� �Ǵ�
    private float Iou(DetectionResult boxA, DetectionResult boxB)
    {
        if ((boxA.x1 == boxB.x1) && (boxA.x2 == boxB.x2) && (boxA.y1 == boxB.y1) && (boxA.y2 == boxB.y2))
        {
            return 1.0f;

        }
        else if (((boxA.x1 <= boxB.x1 && boxA.x2 > boxB.x1) || (boxA.x1 >= boxB.x1 && boxB.x2 > boxA.x1))
          && ((boxA.y1 <= boxB.y1 && boxA.y2 > boxB.y1) || (boxA.y1 >= boxB.y1 && boxB.y2 > boxA.y1)))
        {
            float intersection = (Mathf.Min(boxA.x2, boxB.x2) - Mathf.Max(boxA.x1, boxB.x1))
                * (Mathf.Min(boxA.y2, boxB.y2) - Mathf.Max(boxA.y1, boxB.y1));
            float union = (boxA.x2 - boxA.x1) * (boxA.y2 - boxA.y1) + (boxB.x2 - boxB.x1) * (boxB.y2 - boxB.y1) - intersection;
            return (intersection / union);
        }

        return 0.0f;
    }



    // �̹��� �������� ó��
    private static Texture2D ResizedTexture(Texture2D texture, int width, int height)
    {
        // RenderTexture�� ����
        var rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(texture, rt);
        // RenderTexture���� ������ ��������
        var preRt = RenderTexture.active;
        RenderTexture.active = rt;
        var resizedTexture = new Texture2D(width, height);
        resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = preRt;
        RenderTexture.ReleaseTemporary(rt);
        return resizedTexture;
    }

}

// ���� ���
class DetectionResult
{
    public float x1 { get; }
    public float y1 { get; }
    public float x2 { get; }
    public float y2 { get; }
    public int classId { get; }
    public float score { get; }

    public DetectionResult(Tensor t, int idx)
    {
        // ���� ����� ������� �簢���� ��ǥ ������ 0: �߽� x, 1: �߽� y, 2: ��, 3: ����
        // ��ǥ�踦 �»�� xy, ���ϴ� xy�� ��ȯ
        float halfWidth = t[0, 0, idx, 2] / 2;
        float halfHeight = t[0, 0, idx, 3] / 2;
        x1 = t[0, 0, idx, 0] - halfWidth;
        y1 = t[0, 0, idx, 1] - halfHeight;
        x2 = t[0, 0, idx, 0] + halfWidth;
        y2 = t[0, 0, idx, 1] + halfHeight;

        // ������ �������� �� Ŭ������ ������ �����Ǿ� ����
        // �ִ밪�� �Ǵ��Ͽ� ����
        int classes = t.shape.channels - 4;
        score = 0f;
        for (int i = 0; i < classes; i++)
        {
            float classScore = t[0, 0, idx, i + 4];
            if (classScore < score)
            {
                continue;
            }
            classId = i;
            score = classScore;
        }
    }

}
