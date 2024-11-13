using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LineRenderer linRenderer;
    public Vector3 startPoint;// 始点
    public Vector3 endPoint;// 終点
    public float moveTime = 0.5f; // 移動にかかる時間（秒）
    public float elapsedTime = 0.0f; // 経過時間


    // Update is called once per frame
    void Update()
    {
        Move(); // Moveメソッドを呼び出す
    }

    public void SetUp(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint; // 開始位置を保存
        this.endPoint = endPoint; // 終了位置を保存
        // LineRendererを設定
        linRenderer.positionCount = 2;
        linRenderer.SetPosition(0, startPoint);
        linRenderer.SetPosition(1, endPoint);

        // 初期化
        elapsedTime = 0.0f; // 経過時間をリセット
    }

    // 弾を移動させるロジック
    protected virtual void Move()
    {
        // 時間が経過した場合は位置を更新
        if (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime; // 前フレームからどれだけ時間がたったか
            float t = elapsedTime / moveTime; // 移動がどれだけ進んだかを「0から1」の範囲に変換。正規化された時間。
            Vector3 currentPosition = Vector3.Lerp(startPoint, endPoint, t); // startPoint から endPoint へ滑らかに移動

            // 始点の位置を更新
            linRenderer.SetPosition(0, currentPosition);
            linRenderer.SetPosition(1, endPoint);
        }
        else
        {
            // 移動が完了したら削除
            Destroy(gameObject);
        }
    }
}
