using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // ドアの参照
    public List<Door> connectedDoors; // ボタンが押されたかのフラグ
    public bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ボタンが押されたときに呼び出されるメソッド
    public void OnButtonPressed()
    {
        if (!isPressed)
        {
            isPressed = true; // ボタンが押されたことを記録
            Debug.Log("ボタンが押されました");

            // ドアに通知して状態を更新
            foreach (var door in connectedDoors)
            {
                if (door != null)
                {
                    door.CheckButtons(); // ドアのボタン状態をチェック
                }
            }

            Destroy(gameObject); // 自分自身を削除
        }

    }
}
