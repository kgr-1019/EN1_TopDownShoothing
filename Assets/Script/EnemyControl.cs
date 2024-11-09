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
    private float angle = 90.0f;

    [Header("�v���C���[��T������")]
    private Quaternion lastPlayerDirection; // �Ō�Ƀv���C���[�����Ă�������

    [Header("�e������")]
    public EnemyGun enemyGun; // EnemyGun�R���|�[�l���g���Q��
    [SerializeField]float shotTimer = 0;// �e�����܂ł̎���
    [SerializeField] float reloadTimer = 0;// �����[�h�܂ł̎���

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
        CheckPlayerInView();
        Move();

        // �e�����Ԋu
        shotTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
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
            Debug.Log("����O");
        }
        else
        {
            Debug.Log("�����");

            // Ray���΂�
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction,Color.blue,0.2f);
            int layer = ~LayerMask.GetMask("Item");

            if (Physics.Raycast(ray, out hit,1000,layer))
            {
                Debug.DrawLine(transform.position, hit.point,Color.red,0.3f);
                // Ray���v���C���[�ɓ���������A�v���C���[�������Ă���
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player�ɓ�������");
                    // �v���C���[�����������̃A�N�V����
                    Fire();

                    // �ړ����~�߂�
                    navMeshAgent.isStopped = true;

                    // �v���C���[�̕���������
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookAroundSpeed);
                }
                else
                {
                    // �ړ����~�߂�
                    navMeshAgent.isStopped = false;
                }

            }
        }
    }

    public void Fire()
    {
        if (reloadTimer <= 3.0f)
        {
            // �e������
            if (shotTimer >= 0.5f)
            {
                enemyGun.Shot();
                shotTimer = 0;
            }
        }

        // �����[�h
        if(reloadTimer >= 6.0f)
        {
            reloadTimer = 0;
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
