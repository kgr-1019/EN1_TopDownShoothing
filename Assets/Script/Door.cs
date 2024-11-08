using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Button> connectedButtons = new List<Button>();// �{�^���̏�Ԃ��Ǘ����郊�X�g
    private bool isOpen = false; // �h�A���J���Ă��邩�̃t���O
    public float speed = 1.0f; // �ړ����x
    private float targetY = -3.0f; // �ڕWY���W
    private Vector3 originalPosition; // �h�A�̌��̈ʒu
    private Vector3 targetPosition; // ���݂̖ڕW�ʒu

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position; // ���̈ʒu��ۑ�
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            // ���ɉ�����
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    // �h�A���J���郁�\�b�h
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            targetPosition = new Vector3(originalPosition.x, targetY, originalPosition.z); // �J�����ۂ̖ڕW�ʒu��ݒ�
        }
    }

    // �{�^���������ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void CheckButtons()
    {
        bool allPressed = true;
        
        foreach (var button in connectedButtons)
        {
            if (button != null && !button.isPressed)
            {
                allPressed = false;
                break; // ��ł�������Ă��Ȃ��{�^��������ΏI��
            }
        }
        

        // �S�Ẵ{�^���������ꂽ�ꍇ
        if (allPressed && !isOpen)
        {
            OpenDoor();
        }
    }
}
