using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject gun; // 銃のインスタンス
    private Transform gunTransform; // 銃のトランスフォーム

    
    // 銃をHandの位置にセット
    public void PickUpGun(GameObject pickedGun)
    {
        if (gun == null)
        {
            gun = pickedGun;
            gunTransform = gun.transform;
            // 銃のトランスフォームの親をプレイヤーに設定
            gunTransform.SetParent(transform);
            gunTransform.localPosition = Vector3.zero; // 初期位置をリセットする
            gunTransform.localRotation = Quaternion.identity; // ローカル回転をリセット
        }

        // 銃の有効化
        Gun gunScript = gun.GetComponent<Gun>();
        if (gunScript != null)
        {
            gunScript.hasGun=true;
        }
    }
}
