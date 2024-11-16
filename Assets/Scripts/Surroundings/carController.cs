using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class carController : MonoBehaviour
{
    public NavMeshAgent agent;
    //public Animator animator;

    public GameObject PATH; // Pavewalk
    public Transform[] PathPoints;

    public float minDistance = 10;

    public int index = 0;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();

        // PATH���� ��� �ڽ� ����Ʈ ��������
        PathPoints = new Transform[PATH.transform.childCount];
        for (int i = 0; i < PathPoints.Length; i++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }
    }

    void Update() {
        roam();
    }

    void roam() {
        // ���� PathPoint�� �̵�
        if (Vector3.Distance(transform.position, PathPoints[index].position) < minDistance)
        {
            index = (index + 1) % PathPoints.Length; // ��ȯ ������ �̵�
        }

        agent.SetDestination(PathPoints[index].position);
        //animator.SetFloat("vertical", !agent.isStopped ? 1 : 0); // �ִϸ��̼� ����ȭ
    }
}
