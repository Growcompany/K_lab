using UnityEngine;

public class PIDController
{
    private float kp; // ��� ����
    private float ki; // ���� ����
    private float kd; // �̺� ����

    private float integral; // ���� ���� ��
    private float previousError; // ���� ���� ��

    private float outputMin; // ��� �ּҰ�
    private float outputMax; // ��� �ִ밪

    public PIDController(float kp, float ki, float kd, float outputMin, float outputMax)
    {
        this.kp = kp;
        this.ki = ki;
        this.kd = kd;
        this.outputMin = outputMin;
        this.outputMax = outputMax;

        this.integral = 0f;
        this.previousError = 0f;
    }

    public float Update(float error, float deltaTime)
    {
        // ���� �� ���
        integral += error * deltaTime;

        // �̺� �� ���
        float derivative = (error - previousError) / deltaTime;

        // PID ��� ���
        float output = kp * error + ki * integral + kd * derivative;


        // ��� ����
        output = Mathf.Clamp(output, outputMin, outputMax);

        // ���� ���� ������Ʈ
        previousError = error;

        return output;
    }
}
