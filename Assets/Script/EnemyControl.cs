using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyControl : MonoBehaviour
{

    [SerializeField]
    [Header("巡回する地点の配列")]
    private Transform[] waypointArray;
    private NavMeshAgent navMeshAgent=null;// NavMeshAgentコンポーネントを入れる変数
    private int currentWaypointIndex = 0;// 現在の目的地

    [Header("視野の設定")]
    public Transform player; // プレイヤーのTransform
    private float lookAroundSpeed = 60f; // 振り向く角速度
    private float angle = 90.0f;

    [Header("プレイヤーを探す動作")]
    private float searchingCount = 0;
    private float searchingTimer = 6f;// 何秒探すか
    private float rotateTime = 2f;// どのくらいの間きょろきょろするかのタイマー
    private float searchAngle = 45f;// どのくらい振り向くかの角度
    private bool isLeftRotate = true;// 左に振り向くかどうか
    private float rotateCount = 0; // 片側振り向いてる時間

    [Header("弾を撃つ")]
    public EnemyGun enemyGun; // EnemyGunコンポーネントを参照
    private float shotTimer = 0;// 弾を撃つまでの時間
    private float reloadCoolTimer = 0;// リロードまでの時間

    [Header("HP")]
    private int maxHP = 5;// 最大HP
    private int currentHP;// 現在のHP
    


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
        reloadCoolTimer += Time.deltaTime;

        if (searchingCount > 0)
        {
            Search();
            searchingCount -= Time.deltaTime;
        }
        else
        {
            //Debug.Log("サーチ終了しました");
            navMeshAgent.isStopped = false;
        }
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
            //Debug.Log("視野外");
        }
        else
        {
            //Debug.Log("視野内");

            // Rayを飛ばす
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction,Color.blue,0.2f);
            int layer = ~LayerMask.GetMask("Item");

            if (Physics.Raycast(ray, out hit,12,layer))
            {
                Debug.DrawLine(transform.position, hit.point,Color.red,0.3f);
                // Rayがプレイヤーに当たったら、プレイヤーが見えている
                if (hit.collider.CompareTag("Player"))
                {
                    // 弾を撃つ
                    Fire();

                    // 移動を止める
                    navMeshAgent.isStopped = true;

                    // プレイヤーの方向を向く
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookAroundSpeed);

                    searchingCount = searchingTimer;
                }
            }
        }
        
    }

    // プレイヤーを探す動作
    void Search()
    {
        Debug.Log("サーチ中");

        // 回転角度を計算
        float rotationSpeed = searchAngle / rotateTime * Time.deltaTime;
        float rotationDirection = isLeftRotate ? -rotationSpeed : rotationSpeed;

        // 回転を実行
        transform.Rotate(0.0f, rotationDirection, 0.0f);

        // 回転カウントを更新
        rotateCount += Time.deltaTime;

        // rotateCountがrotateTimeに達したら、左右回転を切り替え
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
            // 弾を撃つ
            if (shotTimer >= 0.5f)
            {
                enemyGun.Shot();
                shotTimer = 0;
            }
        }

        // リロード
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
