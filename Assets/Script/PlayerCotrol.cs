using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotrol : MonoBehaviour
{
    //private Rigidbody rb;
    // 移動
    //private float speed = 0.1f;   // 横に移動する速度

    // Start is called before the first frame update
    void Start()
    {
        // リジッドボディ2Dをコンポーネントから取得して変数に入れる
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 移動処理
        // 左に移動
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.04f, 0.0f, 0.0f);
        }
        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.04f, 0.0f, 0.0f);
        }
        // 前に移動
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, 0.04f);
        }
        // 後ろに移動
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, -0.04f);
        }
    }
}
