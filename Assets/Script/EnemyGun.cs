using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform enemyFirePosition; // 敵の銃口位置
    public Transform player;// プレイヤーのトランスフォーム
    public EnemyBullet bullet;// Bulletクラスへの参照
    private Vector3 hitPos;// Rayが当たる場所
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot()
    {
        EnemyBullet bulletScript = Instantiate(bullet, enemyFirePosition.position, Quaternion.identity);

        RaycastHit hit;
        Ray fireRay = new Ray(enemyFirePosition.position, enemyFirePosition.forward);

        // Rayが当たる処理
        if (Physics.Raycast(fireRay, out hit))
        {
            hitPos = hit.point;
        }
        else
        {
            hitPos = enemyFirePosition.transform.position + enemyFirePosition.transform.forward * 5.0f;
        }

        Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 0.1f);

        
        bulletScript.SetUp(enemyFirePosition.position, hitPos);
    }
}
