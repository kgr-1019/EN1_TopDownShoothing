using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f; // �L�����N�^�[�̈ړ����x
    public Hand handScript; // Hand�N���X�ւ̎Q��
    public float autoMoveSpeed = 2f; // �����ړ����x
    public Clear fadeManager; // FadeManager�N���X�ւ̎Q��

    void Start()
    {
        
    }

    void Update()
    {
        // Z���ʒu��85�𒴂����ꍇ�͎����ړ��̂�
        if (transform.position.z > 85.0f)
        {
            ClearMove();
        }
        else
        {
            Move();
        }

        RotatePlayer();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            handScript.PickUpGun(other.gameObject);
        }
    }
}
