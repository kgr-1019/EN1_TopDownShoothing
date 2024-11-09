using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject gun; // �e�̃C���X�^���X
    private Transform gunTransform; // �e�̃g�����X�t�H�[��
    public GameObject player;// �v���C���[�̃v���n�u

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    // �e��Hand�̈ʒu�ɃZ�b�g
    public void PickUpGun(GameObject pickedGun)
    {
        if (gun == null)
        {
            gun = pickedGun;
            gunTransform = gun.transform;
            // �e�̃g�����X�t�H�[���̐e���v���C���[�ɐݒ�
            gunTransform.SetParent(transform);
            gunTransform.localPosition = Vector3.zero; // �����ʒu�����Z�b�g����
            gunTransform.localRotation = Quaternion.identity; // ���[�J����]�����Z�b�g
        }

        // �e�̗L����
        Gun gunScript = gun.GetComponent<Gun>();
        if (gunScript != null)
        {
            gunScript.hasGun=true;
        }
    }
}
