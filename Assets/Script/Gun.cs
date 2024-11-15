using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform player;// プレイヤーのトランスフォーム
    public GameObject firePosition;// 銃口のプレハブ
    public Bullet bullet;// Bulletクラスへの参照
    private Vector3 hitPos;// Rayが当たる場所
    public bool isParent = false; // ペアレントされているかどうかのフラグ
    public EnemyControl enemy;// EnemyControlクラスへの参照
    private Vector3 direction;// 敵の攻撃関数の引数
    public float shotTimer = 0.5f;
    public float shotCount = 0;
    //public Bom bom;// Bulletクラスへの参照
    private float rotateSpeed = 50f;


    // Update is called once per frame
    public virtual void Update()
    {
        shotCount += Time.deltaTime;

        if (isParent)
        {

            MouseRotateGun();
        }
        else
        {
            RotateGun();
        }
    }

    public void SetParent(bool parent)
    {
        isParent = parent;
    }

    void RotateGun()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void MouseRotateGun()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;

        if (Physics.Raycast(ray, out hitPoint))
        {
            float h = transform.position.y - hitPoint.point.y;
            Vector3 direction = ray.direction * -1;
            float theta = Mathf.Acos(Vector3.Dot(direction, Vector3.up));
            float S = h / Mathf.Cos(theta);
            Vector3 point = hitPoint.point + direction * S;

            transform.LookAt(point);
        }
    }

    public virtual void Shot()
    {

        if (shotCount >= shotTimer)
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
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyControl>().ReduceHP(1);
                }

                if (hit.collider.CompareTag("Bom"))
                {
                    hit.collider.gameObject.GetComponent<Bom>().Explosion();
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


            shotCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.enabled = false;// コンポーネントのアクティブ切り替え
        }
    }
}
