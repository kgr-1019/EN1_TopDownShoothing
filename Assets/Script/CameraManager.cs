using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraManager : MonoBehaviour
{
    [Header("カメラの移動")]
    public PlayerControl player; // プレイヤーのTransformを指定するための変数
    private Vector3 originalPosition; // メインカメラの初期位置を保存する変数
    private Vector3 offset;
    public float maxDistance = 3.0f; // 最大距離を指定するための変数
    public LayerMask groundLayer;

    [Header("シェイク")]
    private float shakeDuration; // シェイクの持続時間
    private float shakeMagnitude; // シェイクの強度
    private float shakeTime; // シェイクの経過時間
    private float decrementMagnitude;// シェイクの減衰

    // Start is called before the first frame update
    void Start()
    {
        // メインカメラの初期位置を保存
        originalPosition = transform.position;

        // 相対距離
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        

        // シェイクが進行中であればシェイク処理を続ける
        if (shakeTime > 0)
        {
            
            // シェイクを減衰させる
            decrementMagnitude = shakeMagnitude * (shakeTime / shakeDuration);

            // ランダムなオフセットを計算
            Vector3 shakeOffset = Random.insideUnitSphere * decrementMagnitude;

            // カメラの新しい位置を設定
            transform.localPosition = originalPosition + shakeOffset;

            // 経過時間を減少させる
            shakeTime -= Time.deltaTime;
        }
        else
        {
            // シェイクが終わったら元の位置に戻す
            transform.localPosition = originalPosition;
        }
    }

    
    void CameraMove()
    {
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit,100,groundLayer))
        {
            Vector3 distance = (hit.point - player.transform.position)*0.3f;

            // 最大距離を超えないようにする
            if(distance.magnitude > maxDistance)
            {
                distance = distance.normalized * maxDistance;
            }

            // カメラの位置を更新
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset + distance, 0.1f);
            originalPosition = transform.position;
        }

    }

    

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration; // シェイクの持続時間を設定
        shakeMagnitude = magnitude; // シェイクの強度を設定
        shakeTime = shakeDuration; // シェイクの経過時間を初期化
    }

}
