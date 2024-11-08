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
    private bool hasGun; // 銃を持っているかどうかのフラグ
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
            RotateTowardsMouse();
        }
    }

    public void EnableGun()
    {
        hasGun = true; // 銃が有効化されたらフラグを設定
    }

    private void RotateTowardsMouse()
    {
        // マウスの位置を世界座標に変換
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;
        if (plane.Raycast(ray, out rayLength))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(rayLength);

            // マウスの位置にキャラクターの正面を向かせる
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            direction.y = 0; // Y軸を0にして平面上の移動にする
            transform.forward = direction; // キャラクターの正面をマウスの方向に向ける
        }
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
