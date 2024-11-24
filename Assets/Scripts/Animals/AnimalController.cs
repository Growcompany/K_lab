using UnityEngine;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimalController : MonoBehaviour
    {
        public Animator animator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private float moveSpeed = 2.0f; // �̵� �ӵ�
        private Rigidbody rb;
        private Vector3 moveDirection; // �̵� ����
        private float lifetime = 10f; // �� �� �� ����

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            // Rigidbody ���� Ȯ��
            if (rb != null)
            {
                rb.useGravity = true; // �߷� Ȱ��ȭ
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // ȸ�� ����
            }

            // Root Motion ��Ȱ��ȭ
            if (animator != null && animator.applyRootMotion)
            {
                animator.applyRootMotion = false;
            }

            // �̵� ���� ����
            SetMoveDirection();

            // �ʱ� �ִϸ��̼� ����
            animator.SetBool(IsWalking, true);

            // �ʱ� �̵� ����
            ApplyInitialMovement();

            // ���� �ð� �� ����
            Destroy(gameObject, lifetime);
        }

        void FixedUpdate()
        {
            MoveForward();
        }

        void Update()
        {
            // �ִϸ��̼� ����
            bool isMoving = rb.velocity.magnitude > 0.1f;
            if (animator != null)
            {
                animator.SetBool(IsWalking, isMoving);
            }
        }

        private void MoveForward()
        {
            if (rb == null) return;

            // XZ ��� �ӵ��� ����, Y���� �߷¿� �ñ�
            Vector3 velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
            rb.velocity = velocity;

            // ��ü�� ���� ������Ʈ
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }

        private void SetMoveDirection()
        {
            // XZ ��鿡�� ���� ���� ����
            moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            Debug.Log($"�̵� ����: {moveDirection}");

            // �ʱ� ���� ����
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }

        private void ApplyInitialMovement()
        {
            // �ʱ� �̵� �ӵ��� �����Ͽ� �ִϸ��̼� �����̸� ����
            rb.velocity = moveDirection * moveSpeed;
        }
    }
}
