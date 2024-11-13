using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LineRenderer linRenderer;
    public Vector3 startPoint;// �n�_
    public Vector3 endPoint;// �I�_
    public float moveTime = 0.5f; // �ړ��ɂ����鎞�ԁi�b�j
    public float elapsedTime = 0.0f; // �o�ߎ���


    // Update is called once per frame
    void Update()
    {
        Move(); // Move���\�b�h���Ăяo��
    }

    public void SetUp(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint; // �J�n�ʒu��ۑ�
        this.endPoint = endPoint; // �I���ʒu��ۑ�
        // LineRenderer��ݒ�
        linRenderer.positionCount = 2;
        linRenderer.SetPosition(0, startPoint);
        linRenderer.SetPosition(1, endPoint);

        // ������
        elapsedTime = 0.0f; // �o�ߎ��Ԃ����Z�b�g
    }

    // �e���ړ������郍�W�b�N
    protected virtual void Move()
    {
        // ���Ԃ��o�߂����ꍇ�͈ʒu���X�V
        if (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime; // �O�t���[������ǂꂾ�����Ԃ���������
            float t = elapsedTime / moveTime; // �ړ����ǂꂾ���i�񂾂����u0����1�v�͈̔͂ɕϊ��B���K�����ꂽ���ԁB
            Vector3 currentPosition = Vector3.Lerp(startPoint, endPoint, t); // startPoint ���� endPoint �֊��炩�Ɉړ�

            // �n�_�̈ʒu���X�V
            linRenderer.SetPosition(0, currentPosition);
            linRenderer.SetPosition(1, endPoint);
        }
        else
        {
            // �ړ�������������폜
            Destroy(gameObject);
        }
    }
}
