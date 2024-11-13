using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Gun gun; // �e�̃C���X�^���X
    public Gun gunScript; // �e�̃C���X�^���X
    private Transform gunTransform; // �e�̃g�����X�t�H�[��


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

    // �e��Hand�̈ʒu�ɃZ�b�g
    public void PickUpGun(Gun pickedGun)
    {
        if (gun != null)
        {
            //Debug.Log("�e�����Ă܂�");
            Destroy(gun.gameObject);
            gun = null;
        }

        gun = pickedGun;
        gunTransform = gun.transform;
        gunTransform.localPosition = Vector3.zero; // ���Έʒu�����Z�b�g
        gunTransform.localRotation = Quaternion.identity; // ���Ή�]�����Z�b�g

        // �e�X�N���v�g��ݒ�
        gunScript = gun.GetComponent<Gun>();

        gunScript.SetParent(true);
    }
}
