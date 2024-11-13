using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // �L�����N�^�[�̈ړ����x
    public Hand handScript; // Hand�N���X�ւ̎Q��
    public GameObject hand;
    public float autoMoveSpeed = 2f; // �����ړ����x
    public Clear fadeManager; // FadeManager�N���X�ւ̎Q��
    public Gun gun;
    

    void Update()
    {
        // Z���ʒu��85�𒴂����ꍇ�͎����ړ��̂�
        if (transform.position.z > 200.0f)
        {
            ClearMove();
        }
        else
        {
            Move();
        }


        RotatePlayer();


        if (handScript.isParent)
        {
            if (Input.GetMouseButton(0))
            {
                handScript.Fire();
            }
        }
    }

    private void Move()
    {
        // WASD�L�[�ł̈ړ�����
        float horizontal = Input.GetAxis("Horizontal"); // A / D�L�[ (�� / �E)
        float vertical = Input.GetAxis("Vertical"); // W / S�L�[ (�O / ��)

        Vector3 movement = new Vector3(horizontal, 0, vertical); // ���̓x�N�g���𐳋K��

        // �L�����N�^�[���ړ�������
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void RotatePlayer()
    {
        // �}�E�X�̈ʒu�����[���h���W�ɕϊ�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float rotateSpeed = 5.0f;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 direction = (targetPoint - transform.position).normalized;
            Quaternion rotate = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed);
        }
    }

    private void ClearMove()
    {
        fadeManager.FadeIn();  // FadeManager�̏������Ăяo��
        transform.position += Vector3.forward * autoMoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Gun"))
        {
            //gunPosition�ɐe�q�t������
            collision.gameObject.transform.SetParent(hand.transform);



            if (collision.gameObject.TryGetComponent(out gun))
            {
                Vector3 gunPosition = new Vector3(0.05f, 0.1f, 0.1f); // �����ŏe�̈ʒu���w��
                Quaternion gunRotation = Quaternion.Euler(0, 0, 0);

                handScript.PickUpGun(gun);
            }
        }
    }
}
