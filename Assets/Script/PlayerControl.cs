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

        RotateTowardsMouse();
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

    private void ClearMove()
    {
        fadeManager.FadeIn();  // FadeManager�̏������Ăяo��
        transform.position += Vector3.forward * autoMoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            handScript.PickUpGun();
            Destroy(other.gameObject);
        }
    }
}
