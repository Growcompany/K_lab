using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWalk : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            // "Locom_m_basic Walk_30f" �ִϸ��̼��� ���
            PlayWalkAnimation();
        }
    }

    void PlayWalkAnimation()
    {
        // �ִϸ��̼� �̸��� ���� �����Ͽ� ���
        animator.CrossFadeInFixedTime("Locom_m_basic Walk_30f", 1.0f);
    }
}
