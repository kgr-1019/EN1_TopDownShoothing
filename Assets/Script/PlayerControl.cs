using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // キャラクターの移動速度
    public Hand handScript; // Handクラスへの参照
    public float autoMoveSpeed = 2f; // 自動移動速度
    public Clear fadeManager; // FadeManagerクラスへの参照

    void Start()
    {
        
    }

    void Update()
    {
        // Z軸位置が85を超えた場合は自動移動のみ
        if (transform.position.z > 85.0f)
        {
            ClearMove();
        }
        else
        {
            Move();
        }

        RotatePlayer();
    }

    private void Move()
    {
        // WASDキーでの移動処理
        float horizontal = Input.GetAxis("Horizontal"); // A / Dキー (左 / 右)
        float vertical = Input.GetAxis("Vertical"); // W / Sキー (前 / 後)

        Vector3 movement = new Vector3(horizontal, 0, vertical); // 入力ベクトルを正規化

        // キャラクターを移動させる
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void RotatePlayer()
    {
        // マウスの位置をワールド座標に変換
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float rotateSpeed = 5.0f;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 direction = (targetPoint - transform.position).normalized;
            Quaternion rotate = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed);
        }
    }

    private void ClearMove()
    {
        fadeManager.FadeIn();  // FadeManagerの処理を呼び出す
        transform.position += Vector3.forward * autoMoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            handScript.PickUpGun(other.gameObject);
        }
    }
}
