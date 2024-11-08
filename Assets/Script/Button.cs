using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // �h�A�̎Q��
    public List<Door> connectedDoors; // �{�^���������ꂽ���̃t���O
    public bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �{�^���������ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnButtonPressed()
    {
        if (!isPressed)
        {
            isPressed = true; // �{�^���������ꂽ���Ƃ��L�^
            Debug.Log("�{�^����������܂���");

            // �h�A�ɒʒm���ď�Ԃ��X�V
            foreach (var door in connectedDoors)
            {
                if (door != null)
                {
                    door.CheckButtons(); // �h�A�̃{�^����Ԃ��`�F�b�N
                }
            }

            Destroy(gameObject); // �������g���폜
        }

    }
}
