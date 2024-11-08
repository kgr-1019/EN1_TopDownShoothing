using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform player;// �v���C���[�̃g�����X�t�H�[��
    public GameObject firePosition;// �e���̃v���n�u
    public Bullet bullet;// Bullet�N���X�ւ̎Q��
    public Vector3 hitPos;// Ray��������ꏊ
    private bool hasGun; // �e�������Ă��邩�ǂ����̃t���O
    public EnemyControl enemy;// EnemyControl�N���X�ւ̎Q��
    private Vector3 direction;// �G�̍U���֐��̈���
    
    private void Awake()
    {
        hasGun = false; // ������Ԃł�Gun�������Ă��Ȃ�
    }

    // Update is called once per frame
    void Update()
    {
        // ���N���b�N������
        if (hasGun && Input.GetMouseButtonDown(0))
        {
            Shot();
        }

        if (hasGun)
        {
            RotateTowardsMouse();
        }
    }

    public void EnableGun()
    {
        hasGun = true; // �e���L�������ꂽ��t���O��ݒ�
    }

    private void RotateTowardsMouse()
    {
        // �}�E�X�̈ʒu�𐢊E���W�ɕϊ�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;
        if (plane.Raycast(ray, out rayLength))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(rayLength);

            // �}�E�X�̈ʒu�ɃL�����N�^�[�̐��ʂ���������
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            direction.y = 0; // Y����0�ɂ��ĕ��ʏ�̈ړ��ɂ���
            transform.forward = direction; // �L�����N�^�[�̐��ʂ��}�E�X�̕����Ɍ�����
        }
    }

    void Shot()
    {
        RaycastHit hit;// //Ray�����������I�u�W�F�N�g�̏�������
        Ray fireRay = new Ray(firePosition.transform.position, firePosition.transform.forward);// �ǂ�����ǂ��Ɍ����Ĕ�΂���

        // ���C���̒���
        Vector3 lineRange = firePosition.transform.forward * 5.0f;

        Vector3 firePos = firePosition.transform.position;// �n�_

        if (Physics.Raycast(fireRay, out hit))
        {
            // ����������
            hitPos = hit.point;// �I�u�W�F�N�g�Ƃ̏Փˍ��W���擾

            if (hit.collider.CompareTag("Button"))
            {
                Button hitButton = hit.collider.GetComponent<Button>();
                if (hitButton != null)
                {
                    // ���������{�^�����R�l�N�g���ꂽ�h�A���J��
                    hitButton.OnButtonPressed();
                }
            }

            // �G��HP�����炷
            if (hit.collider.CompareTag("Enemy"))
            {
                enemy.ReduceHP();
            }
        }
        else
        {
            // ������Ȃ�������
            hitPos = firePosition.transform.position + lineRange; // �I�_
        }

        // Ray�̃V�[���r���[�ł̉���
        Debug.DrawRay(fireRay.origin, fireRay.direction * 100, Color.red, 0.1f);

        // �e�𐶐�
        Bullet bulletScript = Instantiate(bullet, firePos, Quaternion.identity);
        bulletScript.SetUp(firePos, hitPos);// �n�_�ƏI�_��n��
    }
}
