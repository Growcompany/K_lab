using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText; // TextMesh Pro ������Ʈ�� ������ ����
    private float elapsedTime = 0f;  // �帥 �ð�

    void Update()
    {
        // �ð� ���
        elapsedTime += Time.deltaTime;

        // �ð��� ��:��:�� �������� ����
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);

        // �ؽ�Ʈ ������Ʈ
        timeText.text = string.Format("Time: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}

