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
    private float angle = 90.0f;

    [Header("プレイヤーを探す動作")]
    private Quaternion lastPlayerDirection; // 最後にプレイヤーを見ていた向き

    [Header("弾を撃つ")]
    public EnemyGun enemyGun; // EnemyGunコンポーネントを参照
    [SerializeField]float shotTimer = 0;// 弾を撃つまでの時間
    [SerializeField] float reloadTimer = 0;// リロードまでの時間

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
        CheckPlayerInView();
        Move();

        // 弾を撃つ間隔
        shotTimer += Time.deltaTime;
        reloadTimer += Time.deltaTime;
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


        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = transform.position;

        Vector3 direction = (playerPos - enemyPos).normalized;
        float dot = Vector3.Dot(transform.forward, direction);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (theta > angle)
        {
            Debug.Log("視野外");
        }
        else
        {
            Debug.Log("視野内");

            // Rayを飛ばす
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction,Color.blue,0.2f);
            int layer = ~LayerMask.GetMask("Item");

            if (Physics.Raycast(ray, out hit,1000,layer))
            {
                Debug.DrawLine(transform.position, hit.point,Color.red,0.3f);
                // Rayがプレイヤーに当たったら、プレイヤーが見えている
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Playerに当たった");
                    // プレイヤーを見つけた時のアクション
                    Fire();

                    // 移動を止める
                    navMeshAgent.isStopped = true;

                    // プレイヤーの方向を向く
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookAroundSpeed);
                }
                else
                {
                    // 移動を止める
                    navMeshAgent.isStopped = false;
                }

            }
        }
    }

    public void Fire()
    {
        if (reloadTimer <= 3.0f)
        {
            // 弾を撃つ
            if (shotTimer >= 0.5f)
            {
                enemyGun.Shot();
                shotTimer = 0;
            }
        }

        // リロード
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
