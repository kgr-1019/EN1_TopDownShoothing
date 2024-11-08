using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{

    [SerializeField]
    [Header("���񂷂�n�_�̔z��")]
    private Transform[] waypointArray;
    private NavMeshAgent navMeshAgent;// NavMeshAgent�R���|�[�l���g������ϐ�
    private int currentWaypointIndex = 0;// ���݂̖ړI�n

    [Header("����̐ݒ�")]
    public float fieldOfViewAngle = 110f; // ���̊p�x
    public float detectionRange = 15f; // ����̋���
    public Transform player; // �v���C���[��Transform
    public float lookAroundDuration = 1.5f; // �U���������
    public float lookAroundSpeed = 60f; // �U������p���x

    [Header("�v���C���[��T������")]
    private Quaternion lastPlayerDirection; // �Ō�Ƀv���C���[�����Ă�������

    [Header("�e������")]
    public EnemyGun enemyGun; // EnemyGun�R���|�[�l���g���Q��
    [SerializeField]float shotTimer = 0;// �e�����܂ł̎���

    [Header("HP")]
    private int maxHP = 5;// �ő�HP
    private int currentHP;// ���݂�HP
    public Door door;// Door�R���|�[�l���g���Q��

    void Start()
    {
        currentHP = maxHP;// HP�����Z�b�g

        navMeshAgent = GetComponent<NavMeshAgent>();
        // �ŏ��̖ړI�n������
        navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
    }

    void Update()
    {
        //Debug.Log("�T��"+lookAroundTime);
        
        Move();
        CheckPlayerInView();

        // �e�����Ԋu
        shotTimer += Time.deltaTime;
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

        Vector3 directionToPlayer = player.position - transform.position;

        // �v���C���[���������������̋��������m�F
        if (directionToPlayer.magnitude <= detectionRange && IsPlayerVisible(directionToPlayer))
        {
            // �v���C���[�����m�������̃A�N�V����
            Fire(directionToPlayer);
        }
        else
        {
            // �v���C���[���������Ȃ������ꍇ�A�ړ����ĊJ
            navMeshAgent.isStopped = false;
        }
    }

    // �v���C���[����������ǂ���
    bool IsPlayerVisible(Vector3 directionToPlayer)
    {
        //float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        //// �v���C���[����������m�F
        //if (angleToPlayer <= fieldOfViewAngle / 2)
        //{
            RaycastHit hit;
            // ������Ray���΂�
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
            {
                // Ray���v���C���[�ɓ���������A�v���C���[�������Ă���
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        //}
        return false;
    }

    public void Fire(Vector3 directionToPlayer)
    {
        // �ړ����~�߂�
        navMeshAgent.isStopped = true;

        // �v���C���[�̕���������
        lastPlayerDirection = Quaternion.LookRotation(directionToPlayer); // �v���C���[�̕������L�^
        transform.rotation = Quaternion.Slerp(transform.rotation, lastPlayerDirection, Time.deltaTime * 5f);

        // �e������
        if (shotTimer >= 0.5f)
        {
            enemyGun.Shot();
            shotTimer = 0;
        }
    }


    public void ReduceHP()
    {
        currentHP -= 1;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            door.CheckButtons();
        }
    }
}
