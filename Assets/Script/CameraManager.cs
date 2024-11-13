using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraManager : MonoBehaviour
{
    [Header("�J�����̈ړ�")]
    public PlayerControl player; // �v���C���[��Transform���w�肷�邽�߂̕ϐ�
    private Vector3 originalPosition; // ���C���J�����̏����ʒu��ۑ�����ϐ�
    private Vector3 offset;
    public float maxDistance = 3.0f; // �ő勗�����w�肷�邽�߂̕ϐ�
    public LayerMask groundLayer;

    [Header("�V�F�C�N")]
    private float shakeDuration; // �V�F�C�N�̎�������
    private float shakeMagnitude; // �V�F�C�N�̋��x
    private float shakeTime; // �V�F�C�N�̌o�ߎ���
    private float decrementMagnitude;// �V�F�C�N�̌���

    // Start is called before the first frame update
    void Start()
    {
        // ���C���J�����̏����ʒu��ۑ�
        originalPosition = transform.position;

        // ���΋���
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        

        // �V�F�C�N���i�s���ł���΃V�F�C�N�����𑱂���
        if (shakeTime > 0)
        {
            
            // �V�F�C�N������������
            decrementMagnitude = shakeMagnitude * (shakeTime / shakeDuration);

            // �����_���ȃI�t�Z�b�g���v�Z
            Vector3 shakeOffset = Random.insideUnitSphere * decrementMagnitude;

            // �J�����̐V�����ʒu��ݒ�
            transform.localPosition = originalPosition + shakeOffset;

            // �o�ߎ��Ԃ�����������
            shakeTime -= Time.deltaTime;
        }
        else
        {
            // �V�F�C�N���I������猳�̈ʒu�ɖ߂�
            transform.localPosition = originalPosition;
        }
    }

    
    void CameraMove()
    {
        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit,100,groundLayer))
        {
            Vector3 distance = (hit.point - player.transform.position)*0.3f;

            // �ő勗���𒴂��Ȃ��悤�ɂ���
            if(distance.magnitude > maxDistance)
            {
                distance = distance.normalized * maxDistance;
            }

            // �J�����̈ʒu���X�V
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset + distance, 0.1f);
            originalPosition = transform.position;
        }

    }

    

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration; // �V�F�C�N�̎������Ԃ�ݒ�
        shakeMagnitude = magnitude; // �V�F�C�N�̋��x��ݒ�
        shakeTime = shakeDuration; // �V�F�C�N�̌o�ߎ��Ԃ�������
    }

}
