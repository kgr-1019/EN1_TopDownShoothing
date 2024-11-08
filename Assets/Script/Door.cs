using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Button> connectedButtons = new List<Button>();// ボタンの状態を管理するリスト
    private bool isOpen = false; // ドアが開いているかのフラグ
    public float speed = 1.0f; // 移動速度
    private float targetY = -3.0f; // 目標Y座標
    private Vector3 originalPosition; // ドアの元の位置
    private Vector3 targetPosition; // 現在の目標位置

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position; // 元の位置を保存
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            // 下に下がる
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    // ドアを開けるメソッド
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            targetPosition = new Vector3(originalPosition.x, targetY, originalPosition.z); // 開いた際の目標位置を設定
        }
    }

    // ボタンが押されたときに呼び出されるメソッド
    public void CheckButtons()
    {
        bool allPressed = true;
        
        foreach (var button in connectedButtons)
        {
            if (button != null && !button.isPressed)
            {
                allPressed = false;
                break; // 一つでも押されていないボタンがあれば終了
            }
        }
        

        // 全てのボタンが押された場合
        if (allPressed && !isOpen)
        {
            OpenDoor();
        }
    }
}
