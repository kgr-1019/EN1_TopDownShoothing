using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<GameObject> connectedButtons = new List<GameObject>();// �{�^���̏�Ԃ��Ǘ����郊�X�g
    //private bool isOpen = false; // �h�A���J���Ă��邩�̃t���O
    public float speed = 1.0f; // �ړ����x
    private float targetY = -6.0f; // �ڕWY���W
    private Vector3 originalPosition; // �h�A�̌��̈ʒu
    private Vector3 targetPosition; // ���݂̖ڕW�ʒu
    bool allPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position; // ���̈ʒu��ۑ�
    }

    // Update is called once per frame
    void Update()
    {
        

        for (int i = 0; i < connectedButtons.Count; i++)
        {
            if(connectedButtons[i] != null)
            {
                allPressed = false;
                break;
            }
            else
            {
                allPressed = true;

            }
        }

        if (allPressed)
        {
            // ���ɉ�����
            targetPosition = new Vector3(originalPosition.x, targetY, originalPosition.z); // �J�����ۂ̖ڕW�ʒu��ݒ�
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if (transform.position.y <= -6.0f) 
        {
            Destroy(gameObject); 
        }
    }
}
