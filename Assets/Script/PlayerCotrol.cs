using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotrol : MonoBehaviour
{
    //private Rigidbody rb;
    // �ړ�
    //private float speed = 0.1f;   // ���Ɉړ����鑬�x

    // Start is called before the first frame update
    void Start()
    {
        // ���W�b�h�{�f�B2D���R���|�[�l���g����擾���ĕϐ��ɓ����
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // �ړ�����
        // ���Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.04f, 0.0f, 0.0f);
        }
        // �E�Ɉړ�
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.04f, 0.0f, 0.0f);
        }
        // �O�Ɉړ�
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, 0.04f);
        }
        // ���Ɉړ�
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, -0.04f);
        }
    }
}
