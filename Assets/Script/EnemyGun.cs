using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    private const float kReloadCoolTime = 5;// クールタイム
    private float reloadCoolTimer = 0;// リロードまでの時間
    private int bulletCount = 0;// 撃った回数
    private const int kMaxBullets = 3;// 何発撃つか

    private void Start()
    {
        shotTimer = 0.2f;
    }

    // Update is called once per frame
    public override void Update()
    {
        shotCount += Time.deltaTime;
        if (reloadCoolTimer <= 0.0f) { return; }
        reloadCoolTimer -= Time.deltaTime;
        //base.Update();
    }

    public override void Shot()
    {
        if (reloadCoolTimer > 0.0f) { return; }
        // 弾を撃つ
        if (shotCount >= shotTimer)
        {
            base.Shot();
            shotCount = 0.0f;// カウントリセット
            --bulletCount;// 撃ったら減らす
        }
        if (bulletCount <= 0)// 三発撃ち切ったら
        {
            bulletCount = kMaxBullets;// リセット
            reloadCoolTimer = kReloadCoolTime;
        }
    }
}
