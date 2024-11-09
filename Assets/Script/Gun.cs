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
    public bool hasGun; // �e�������Ă��邩�ǂ����̃t���O
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
            RotateGun();
        }
    }

    

    private void RotateGun()
    {
        // �}�E�X�̈ʒu�𐢊E���W�ɕϊ�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;

        if (Physics.Raycast(ray, out hitPoint))
        {
            float h = transform.position.y;
            Vector3 direction = ray.direction * -1;
            float theta = Mathf.Acos(Vector3.Dot(direction, Vector3.up));
            float S = h / Mathf.Cos(theta);
            Vector3 point = (hitPoint.point + direction) * S;

            transform.LookAt(point);

        }
        // Ray�̃V�[���r���[�ł̉���
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.1f);
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
