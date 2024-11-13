using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<GameObject> connectedButtons = new List<GameObject>();// ボタンの状態を管理するリスト
    //private bool isOpen = false; // ドアが開いているかのフラグ
    public float speed = 1.0f; // 移動速度
    private float targetY = -6.0f; // 目標Y座標
    private Vector3 originalPosition; // ドアの元の位置
    private Vector3 targetPosition; // 現在の目標位置
    bool allPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position; // 元の位置を保存
    }

    // Update is called once per frame
    void Update()
    {
        

        for (int i = 0; i < connectedButtons.Count; i++)
        {
            if(connectedButtons[i] != null)
            {
                allPressed = false;
                break;
            }
            else
            {
                allPressed = true;

            }
        }

        if (allPressed)
        {
            // 下に下がる
            targetPosition = new Vector3(originalPosition.x, targetY, originalPosition.z); // 開いた際の目標位置を設定
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if (transform.position.y <= -6.0f) 
        {
            Destroy(gameObject); 
        }
    }
}
