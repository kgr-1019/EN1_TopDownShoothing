using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform player;// プレイヤーのトランスフォーム
    public GameObject firePosition;// 銃口のプレハブ
    public Bullet bullet;// Bulletクラスへの参照
    public Vector3 hitPos;// Rayが当たる場所
    public bool hasGun; // 銃を持っているかどうかのフラグ
    public EnemyControl enemy;// EnemyControlクラスへの参照
    private Vector3 direction;// 敵の攻撃関数の引数
    
    private void Awake()
    {
        hasGun = false; // 初期状態ではGunを持っていない
    }

    // Update is called once per frame
    void Update()
    {
        // 左クリックしたら
        if (hasGun && Input.GetMouseButtonDown(0))
        {
            Shot();
        }

        if (hasGun)
        {
            RotateGun();
        }
    }

    

    private void RotateGun()
    {
        // マウスの位置を世界座標に変換
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;

        if (Physics.Raycast(ray, out hitPoint))
        {
            float h = transform.position.y;
            Vector3 direction = ray.direction * -1;
            float theta = Mathf.Acos(Vector3.Dot(direction, Vector3.up));
            float S = h / Mathf.Cos(theta);
            Vector3 point = (hitPoint.point + direction) * S;

            transform.LookAt(point);

        }
        // Rayのシーンビューでの可視化
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.1f);
    }

    void Shot()
    {
        RaycastHit hit;// //Rayが当たったオブジェクトの情報を入れる
        Ray fireRay = new Ray(firePosition.transform.position, firePosition.transform.forward);// どこからどこに向けて飛ばすか

        // ラインの長さ
        Vector3 lineRange = firePosition.transform.forward * 5.0f;

        Vector3 firePos = firePosition.transform.position;// 始点

        if (Physics.Raycast(fireRay, out hit))
        {
            // 当たったら
            hitPos = hit.point;// オブジェクトとの衝突座標を取得

            if (hit.collider.CompareTag("Button"))
            {
                Button hitButton = hit.collider.GetComponent<Button>();
                if (hitButton != null)
                {
                    // 当たったボタンがコネクトされたドアを開く
                    hitButton.OnButtonPressed();
                }
            }

            // 敵のHPを減らす
            if (hit.collider.CompareTag("Enemy"))
            {
                enemy.ReduceHP();
            }
        }
        else
        {
            // 当たらなかったら
            hitPos = firePosition.transform.position + lineRange; // 終点
        }

        // Rayのシーンビューでの可視化
        Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 0.1f);

        // 弾を生成
        Bullet bulletScript = Instantiate(bullet, firePos, Quaternion.identity);
        bulletScript.SetUp(firePos, hitPos);// 始点と終点を渡す
    }
}
