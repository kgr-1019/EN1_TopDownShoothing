using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player; // プレイヤーのTransformを指定するための変数
    public Camera mainCamera; // メインカメラを指定するための変数
    private GameObject centerObject; // Centerオブジェクトのインスタンス
    private Vector3 initialCameraPosition; // メインカメラの初期位置を保存する変数
    public float maxDistance = 3.0f; // 最大距離を指定するための変数

    // Start is called before the first frame update
    void Start()
    {
      
        // メインカメラの初期位置を保存
        if (mainCamera != null)
        {
            initialCameraPosition = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCenterPoint();
    }

    // プレイヤーとマウスカーソルの位置の中心を計算する関数
    void CalculateCenterPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f; // カメラからの奥行き（必要に応じて調整してください）
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 playerPos = player.position;
        Vector3 centerPoint = new Vector3(
            (worldMousePos.x + playerPos.x) / 2,
            (worldMousePos.y + playerPos.y) / 2,
            Mathf.Lerp(worldMousePos.z, playerPos.z, 0.5f)
        );

        // 現在のプレイヤー位置からセンターポイントまでの距離を計算
        float distanceToCenter = Vector3.Distance(playerPos, centerPoint);

        // 最大距離を超えないように中心点を制限
        if (distanceToCenter > maxDistance)
        {
            Vector3 direction = (centerPoint - playerPos).normalized;
            centerPoint = playerPos + direction * maxDistance;
        }

        // カメラの位置を更新
        if (mainCamera != null)
        {
            // カメラをプレイヤーの位置に基づいて適切に配置
            mainCamera.transform.position = new Vector3(centerPoint.x, initialCameraPosition.y, centerPoint.z - 5.0f); // Y軸は初期値を使用、Z軸はプレイヤーに対する一定の距離
        }
    }

}
