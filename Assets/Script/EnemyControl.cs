using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{

    [SerializeField]
    [Header("巡回する地点の配列")]
    private Transform[] waypointArray;
    private NavMeshAgent navMeshAgent;// NavMeshAgentコンポーネントを入れる変数
    private int currentWaypointIndex = 0;// 現在の目的地

    [Header("視野の設定")]
    public float fieldOfViewAngle = 110f; // 扇状の角度
    public float detectionRange = 15f; // 視野の距離
    public Transform player; // プレイヤーのTransform
    public float lookAroundDuration = 1.5f; // 振り向く時間
    public float lookAroundSpeed = 60f; // 振り向く角速度

    [Header("プレイヤーを探す動作")]
    private Quaternion lastPlayerDirection; // 最後にプレイヤーを見ていた向き

    [Header("弾を撃つ")]
    public EnemyGun enemyGun; // EnemyGunコンポーネントを参照
    [SerializeField]float shotTimer = 0;// 弾を撃つまでの時間

    [Header("HP")]
    private int maxHP = 5;// 最大HP
    private int currentHP;// 現在のHP
    public Door door;// Doorコンポーネントを参照

    void Start()
    {
        currentHP = maxHP;// HPをリセット

        navMeshAgent = GetComponent<NavMeshAgent>();
        // 最初の目的地を入れる
        navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
    }

    void Update()
    {
        //Debug.Log("探す"+lookAroundTime);
        
        Move();
        CheckPlayerInView();

        // 弾を撃つ間隔
        shotTimer += Time.deltaTime;
    }
    
    void Move()
    {
        // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // 目的地の番号を１更新（右辺を剰余演算子にすることで目的地をループさせれる）
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointArray.Length;
            // 目的地を次の場所に設定
            navMeshAgent.SetDestination(waypointArray[currentWaypointIndex].position);
        }
    }

    void CheckPlayerInView()
    {
        // プレイヤーが存在しない場合は何もしない
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;

        // プレイヤーが視野内かつ視野内の距離内か確認
        if (directionToPlayer.magnitude <= detectionRange && IsPlayerVisible(directionToPlayer))
        {
            // プレイヤーを検知した時のアクション
            Fire(directionToPlayer);
        }
        else
        {
            // プレイヤーを見つけられなかった場合、移動を再開
            navMeshAgent.isStopped = false;
        }
    }

    // プレイヤーが視野内かどうか
    bool IsPlayerVisible(Vector3 directionToPlayer)
    {
        //float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        //// プレイヤーが視野内か確認
        //if (angleToPlayer <= fieldOfViewAngle / 2)
        //{
            RaycastHit hit;
            // 方向にRayを飛ばす
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
            {
                // Rayがプレイヤーに当たったら、プレイヤーが見えている
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
        // 移動を止める
        navMeshAgent.isStopped = true;

        // プレイヤーの方向を向く
        lastPlayerDirection = Quaternion.LookRotation(directionToPlayer); // プレイヤーの方向を記録
        transform.rotation = Quaternion.Slerp(transform.rotation, lastPlayerDirection, Time.deltaTime * 5f);

        // 弾を撃つ
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
