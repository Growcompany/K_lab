using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcController_Pavewalk : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public float minDistance = 10; // �Ÿ� ����

    private int index = 0;
    private bool forward = true; // true: ������, false: ������

    public GameObject CROSSWALK; // Crosswalk
    public Transform[] CrosswalkPoints;

    private float timer = 0f;
    private int state = 0; // 0 = ���� ������, 1 = ���� �ʷϺ�, 2 = ���� �����

    private bool isCrossing = false; // �����ڰ� Ⱦ�ܺ����� �ִ��� ����

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // CROSSWALK���� ��� �ڽ� ����Ʈ ��������
        CrosswalkPoints = new Transform[CROSSWALK.transform.childCount];
        for (int i = 0; i < CrosswalkPoints.Length; i++)
        {
            CrosswalkPoints[i] = CROSSWALK.transform.GetChild(i);
        }
    }

    void Update()
    {
        crosswalk(); // ��ȣ�� ���� Ȯ��
    }

    void crosswalk()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case 0: // ���� ������, ������ �ʷϺ�

                if (Vector3.Distance(transform.position, CrosswalkPoints[index].position) < minDistance)
                {
                    index = (index + 1) % CrosswalkPoints.Length; // ��ȯ ������ �̵�
                }

                agent.SetDestination(CrosswalkPoints[index].position);
                animator.SetFloat("vertical", !agent.isStopped ? 1 : 0);

                
                if (timer > 10f) // 10�� �Ŀ� ���� ��ȯ
                {
                    timer = 0f;
                    state = 1; // ���� ���·� ����
                }
                break;

            case 1: // ���� �ʷϺ�, ������ ������
            case 2: // ���� �����, ������ ������
                animator.SetFloat("vertical", 0);
                if ((state == 1 && timer > 17f) || (state == 2 && timer > 3f))
                {
                    timer = 0f;
                    state = (state == 1) ? 2 : 0; // ���� ���·� ��ȯ
                }
                break;
        }
    }
}
