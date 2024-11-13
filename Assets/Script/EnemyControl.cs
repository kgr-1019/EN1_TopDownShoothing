using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyControl : MonoBehaviour
{

    [SerializeField]
    [Header("���񂷂�n�_�̔z��")]
    private Transform[] waypointArray;
    private NavMeshAgent navMeshAgent=null;// NavMeshAgent�R���|�[�l���g������ϐ�
    private int currentWaypointIndex = 0;// ���݂̖ړI�n

    [Header("����̐ݒ�")]
    public Transform player; // �v���C���[��Transform
    private float lookAroundSpeed = 60f; // �U������p���x
    private float angle = 90.0f;

    [Header("�v���C���[��T������")]
    private float searchingCount = 0;
    private float searchingTimer = 6f;// ���b�T����
    private float rotateTime = 2f;// �ǂ̂��炢�̊Ԃ���낫��낷�邩�̃^�C�}�[
    private float searchAngle = 45f;// �ǂ̂��炢�U��������̊p�x
    private bool isLeftRotate = true;// ���ɐU��������ǂ���
    private float rotateCount = 0; // �Б��U������Ă鎞��

    [Header("�e������")]
    public EnemyGun enemyGun; // EnemyGun�R���|�[�l���g���Q��
    private float shotTimer = 0;// �e�����܂ł̎���
    private float reloadCoolTimer = 0;// �����[�h�܂ł̎���

    [Header("HP")]
    private int maxHP = 5;// �ő�HP
    private int currentHP;// ���݂�HP
    


    void Start()
    {
        currentHP = maxHP;// HP�����Z�b�g

        navMeshAgent = GetComponent<NavMeshAgent>();

        
        // �ŏ��̖ړI�n������
        navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
    }

    void Update()
    {
        CheckPlayerInView();
        Move();

        // �e�����Ԋu
        shotTimer += Time.deltaTime;
        reloadCoolTimer += Time.deltaTime;

        if (searchingCount > 0)
        {
            Search();
            searchingCount -= Time.deltaTime;
        }
        else
        {
            //Debug.Log("�T�[�`�I�����܂���");
            navMeshAgent.isStopped = false;
        }
    }
    
    void Move()
    {
        // �ړI�n�_�܂ł̋���(remainingDistance)���ړI�n�̎�O�܂ł̋���(stoppingDistance)�ȉ��ɂȂ�����
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // �ړI�n�̔ԍ����P�X�V�i�E�ӂ���]���Z�q�ɂ��邱�ƂŖړI�n�����[�v�������j
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointArray.Length;
            // �ړI�n�����̏ꏊ�ɐݒ�
            navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
        }
    }

    void CheckPlayerInView()
    {
        // �v���C���[�����݂��Ȃ��ꍇ�͉������Ȃ�
        if (player == null) return;


        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = transform.position;

        Vector3 direction = (playerPos - enemyPos).normalized;
        float dot = Vector3.Dot(transform.forward, direction);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (theta > angle)
        {
            //Debug.Log("����O");
        }
        else
        {
            //Debug.Log("�����");

            // Ray���΂�
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction,Color.blue,0.2f);
            int layer = ~LayerMask.GetMask("Item");

            if (Physics.Raycast(ray, out hit,12,layer))
            {
                Debug.DrawLine(transform.position, hit.point,Color.red,0.3f);
                // Ray���v���C���[�ɓ���������A�v���C���[�������Ă���
                if (hit.collider.CompareTag("Player"))
                {
                    // �e������
                    Fire();

                    // �ړ����~�߂�
                    navMeshAgent.isStopped = true;

                    // �v���C���[�̕���������
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookAroundSpeed);

                    searchingCount = searchingTimer;
                }
            }
        }
        
    }

    // �v���C���[��T������
    void Search()
    {
        Debug.Log("�T�[�`��");

        // ��]�p�x���v�Z
        float rotationSpeed = searchAngle / rotateTime * Time.deltaTime;
        float rotationDirection = isLeftRotate ? -rotationSpeed : rotationSpeed;

        // ��]�����s
        transform.Rotate(0.0f, rotationDirection, 0.0f);

        // ��]�J�E���g���X�V
        rotateCount += Time.deltaTime;

        // rotateCount��rotateTime�ɒB������A���E��]��؂�ւ�
        if (rotateCount >= rotateTime)
        {
            isLeftRotate = !isLeftRotate;
            rotateCount = 0.0f;
        }
    }


    public void Fire()
    {
        if (reloadCoolTimer <= 3.0f)
        {
            // �e������
            if (shotTimer >= 0.5f)
            {
                enemyGun.Shot();
                shotTimer = 0;
            }
        }

        // �����[�h
        if(reloadCoolTimer >= 5.0f)
        {
            reloadCoolTimer = 0;
        }
    }


    public void ReduceHP(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
