using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class AutonomousVehicleController : MonoBehaviour
{
    private VPStandardInput vpInput;
    private LaneDetection laneDetection;
    private TrafficLightDetection trafficLightDetection;

    private bool isAutonomousMode;

    private void Start()
    {
        vpInput = FindObjectOfType<VPStandardInput>();
        laneDetection = FindObjectOfType<LaneDetection>();
        trafficLightDetection = FindObjectOfType<TrafficLightDetection>();
    }

    private void Update()
    {
        if (isAutonomousMode)
        {
            // ���� ������ ��Ƽ� ����
            vpInput.externalSteer = laneDetection.GetSteeringAngle();

            // ��ȣ�� ���¿� ���� ���� ����
            if (trafficLightDetection.IsRedLight())
            {
                vpInput.externalThrottle = 0f; // �������̸� ����
                vpInput.externalBrake = 1f; // �극��ũ
            }
            else
            {
                vpInput.externalThrottle = 0.3f; // �ʷϺ��̸� ���
                vpInput.externalBrake = 0f;
            }
        }
        else
        {
            vpInput.externalThrottle = 0f;
            vpInput.externalSteer = 0f;
        }
    }

    public void SetAutonomousMode(bool enable)
    {
        isAutonomousMode = enable;
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class AutonomousVehicleController : MonoBehaviour
{
    private VPStandardInput vpInput;
    private LaneDetection laneDetection;
    private TrafficLightDetection TrafficLightDetection;

    private bool isAutonomousMode;

    private void Start()
    {
        // VPStandardInput ������Ʈ�� �����ɴϴ�.
        vpInput = FindObjectOfType<VPStandardInput>();
        laneDetection = FindObjectOfType<LaneDetection>();

        // �� ����
        if (vpInput != null)
        {
            //vpInput.externalThrottle = 0.8f; // ����Ʋ �� 0.8
            //vpInput.externalBrake = 0.0f; // �극��ũ�� 0
            //vpInput.externalSteer = -0.5f; // ���� �� -0.5
        }
    }

    private void Update()
    {
        if (isAutonomousMode)
        {
            vpInput.externalThrottle = 0.3f; // ����Ʋ ���� 0.8�� ����
            vpInput.externalSteer = laneDetection.GetSteeringAngle(); // ���� ���� ������ ��Ƽ� ����
        }
        else
        {
            vpInput.externalThrottle = 0f;
            vpInput.externalSteer = 0f;
        }
    }

    public void SetAutonomousMode(bool enable)
    {
        isAutonomousMode = enable;
    }

}*/