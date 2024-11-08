using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject gun; // �e�̃C���X�^���X
    private Transform gunTransform; // �e�̃g�����X�t�H�[��
    public GameObject gunPrefab; // �e�̃v���n�u
    public GameObject player;// �v���C���[�̃v���n�u

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �e�������Ă���ꍇ�A�v���C���[�̉E�O���Ɉʒu�𒲐�
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
            // �e�̃g�����X�t�H�[���̐e���v���C���[�ɐݒ�
            gunTransform.SetParent(transform);
            gunTransform.localPosition = Vector3.zero; // �����ʒu�����Z�b�g����
        }

        // �e�̗L����
        Gun gunScript = gun.GetComponent<Gun>();
        if (gunScript != null)
        {
            gunScript.EnableGun();
        }
    }

    private void UpdateGunPosition()
    {
        // �v���C���[�̉E�O���ɏe��z�u
        Vector3 offset = player.transform.right * 0.1f + player.transform.forward * 0.1f; // �����������I�t�Z�b�g
        gunTransform.position = transform.position + offset;
    }
}
