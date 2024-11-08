using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform���w�肷�邽�߂̕ϐ�
    public Camera mainCamera; // ���C���J�������w�肷�邽�߂̕ϐ�
    private GameObject centerObject; // Center�I�u�W�F�N�g�̃C���X�^���X
    private Vector3 initialCameraPosition; // ���C���J�����̏����ʒu��ۑ�����ϐ�
    public float maxDistance = 3.0f; // �ő勗�����w�肷�邽�߂̕ϐ�

    // Start is called before the first frame update
    void Start()
    {
      
        // ���C���J�����̏����ʒu��ۑ�
        if (mainCamera != null)
        {
            initialCameraPosition = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCenterPoint();
    }

    // �v���C���[�ƃ}�E�X�J�[�\���̈ʒu�̒��S���v�Z����֐�
    void CalculateCenterPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f; // �J��������̉��s���i�K�v�ɉ����Ē������Ă��������j
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 playerPos = player.position;
        Vector3 centerPoint = new Vector3(
            (worldMousePos.x + playerPos.x) / 2,
            (worldMousePos.y + playerPos.y) / 2,
            Mathf.Lerp(worldMousePos.z, playerPos.z, 0.5f)
        );

        // ���݂̃v���C���[�ʒu����Z���^�[�|�C���g�܂ł̋������v�Z
        float distanceToCenter = Vector3.Distance(playerPos, centerPoint);

        // �ő勗���𒴂��Ȃ��悤�ɒ��S�_�𐧌�
        if (distanceToCenter > maxDistance)
        {
            Vector3 direction = (centerPoint - playerPos).normalized;
            centerPoint = playerPos + direction * maxDistance;
        }

        // �J�����̈ʒu���X�V
        if (mainCamera != null)
        {
            // �J�������v���C���[�̈ʒu�Ɋ�Â��ēK�؂ɔz�u
            mainCamera.transform.position = new Vector3(centerPoint.x, initialCameraPosition.y, centerPoint.z - 5.0f); // Y���͏����l���g�p�AZ���̓v���C���[�ɑ΂�����̋���
        }
    }

}
