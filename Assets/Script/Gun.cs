using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform player;// �v���C���[�̃g�����X�t�H�[��
    public GameObject firePosition;// �e���̃v���n�u
    public Bullet bullet;// Bullet�N���X�ւ̎Q��
    private Vector3 hitPos;// Ray��������ꏊ
    public bool isParent = false; // �y�A�����g����Ă��邩�ǂ����̃t���O
    public EnemyControl enemy;// EnemyControl�N���X�ւ̎Q��
    private Vector3 direction;// �G�̍U���֐��̈���
    public float shotTimer = 0.5f;
    public float shotCount = 0;
    //public Bom bom;// Bullet�N���X�ւ̎Q��
    private float rotateSpeed = 50f;


    // Update is called once per frame
    public virtual void Update()
    {
        shotCount += Time.deltaTime;

        if (isParent)
        {

            MouseRotateGun();
        }
        else
        {
            RotateGun();
        }
    }

    public void SetParent(bool parent)
    {
        isParent = parent;
    }

    void RotateGun()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void MouseRotateGun()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;

        if (Physics.Raycast(ray, out hitPoint))
        {
            float h = transform.position.y - hitPoint.point.y;
            Vector3 direction = ray.direction * -1;
            float theta = Mathf.Acos(Vector3.Dot(direction, Vector3.up));
            float S = h / Mathf.Cos(theta);
            Vector3 point = hitPoint.point + direction * S;

            transform.LookAt(point);
        }
    }

    public virtual void Shot()
    {

        if (shotCount >= shotTimer)
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
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyControl>().ReduceHP(1);
                }

                if (hit.collider.CompareTag("Bom"))
                {
                    hit.collider.gameObject.GetComponent<Bom>().Explosion();
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


            shotCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.enabled = false;// �R���|�[�l���g�̃A�N�e�B�u�؂�ւ�
        }
    }
}
