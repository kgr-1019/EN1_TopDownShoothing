using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject gun; // 銃のインスタンス
    private Transform gunTransform; // 銃のトランスフォーム
    public GameObject gunPrefab; // 銃のプレハブ
    public GameObject player;// プレイヤーのプレハブ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 銃を持っている場合、プレイヤーの右前方に位置を調整
        if (gun != null)
        {
            UpdateGunPosition();
        }
    }


    public void PickUpGun()
    {
        if (gun == null)
        {
            gun = Instantiate(gunPrefab, transform);
            gunTransform = gun.transform;
            // 銃のトランスフォームの親をプレイヤーに設定
            gunTransform.SetParent(transform);
            gunTransform.localPosition = Vector3.zero; // 初期位置をリセットする
        }

        // 銃の有効化
        Gun gunScript = gun.GetComponent<Gun>();
        if (gunScript != null)
        {
            gunScript.EnableGun();
        }
    }

    private void UpdateGunPosition()
    {
        // プレイヤーの右前方に銃を配置
        Vector3 offset = player.transform.right * 0.1f + player.transform.forward * 0.1f; // 調整したいオフセット
        gunTransform.position = transform.position + offset;
    }
}
