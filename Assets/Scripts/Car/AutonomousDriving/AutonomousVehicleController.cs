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
            // 차선 정보로 스티어링 조정
            vpInput.externalSteer = laneDetection.GetSteeringAngle();

            // 신호등 상태에 따라 차량 제어
            if (trafficLightDetection.IsRedLight())
            {
                vpInput.externalThrottle = 0f; // 빨간불이면 정지
                vpInput.externalBrake = 1f; // 브레이크
            }
            else
            {
                vpInput.externalThrottle = 0.3f; // 초록불이면 출발
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
        // VPStandardInput 컴포넌트를 가져옵니다.
        vpInput = FindObjectOfType<VPStandardInput>();
        laneDetection = FindObjectOfType<LaneDetection>();

        // 값 예시
        if (vpInput != null)
        {
            //vpInput.externalThrottle = 0.8f; // 스로틀 값 0.8
            //vpInput.externalBrake = 0.0f; // 브레이크를 0
            //vpInput.externalSteer = -0.5f; // 조향 값 -0.5
        }
    }

    private void Update()
    {
        if (isAutonomousMode)
        {
            vpInput.externalThrottle = 0.3f; // 스로틀 값을 0.8로 설정
            vpInput.externalSteer = laneDetection.GetSteeringAngle(); // 차선 검출 값으로 스티어링 설정
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