using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Gun gun; // 銃のインスタンス
    public Gun gunScript; // 銃のインスタンス
    private Transform gunTransform; // 銃のトランスフォーム


    public bool isParent
    {
        get { return transform.childCount >= 1; }
    }

    public void Fire()
    {
        if (gun != null)
        {
            gunScript.Shot();
        }
    }

    // 銃をHandの位置にセット
    public void PickUpGun(Gun pickedGun)
    {
        if (gun != null)
        {
            //Debug.Log("銃持ってます");
            Destroy(gun.gameObject);
            gun = null;
        }

        gun = pickedGun;
        gunTransform = gun.transform;
        gunTransform.localPosition = Vector3.zero; // 相対位置をリセット
        gunTransform.localRotation = Quaternion.identity; // 相対回転をリセット

        // 銃スクリプトを設定
        gunScript = gun.GetComponent<Gun>();

        gunScript.SetParent(true);
    }
}
