using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    private const float kReloadCoolTime = 5;// �N�[���^�C��
    private float reloadCoolTimer = 0;// �����[�h�܂ł̎���
    private int bulletCount = 0;// ��������
    private const int kMaxBullets = 3;// ��������

    private void Start()
    {
        shotTimer = 0.2f;
    }

    // Update is called once per frame
    public override void Update()
    {
        shotCount += Time.deltaTime;
        if (reloadCoolTimer <= 0.0f) { return; }
        reloadCoolTimer -= Time.deltaTime;
        //base.Update();
    }

    public override void Shot()
    {
        if (reloadCoolTimer > 0.0f) { return; }
        // �e������
        if (shotCount >= shotTimer)
        {
            base.Shot();
            shotCount = 0.0f;// �J�E���g���Z�b�g
            --bulletCount;// �������猸�炷
        }
        if (bulletCount <= 0)// �O�������؂�����
        {
            bulletCount = kMaxBullets;// ���Z�b�g
            reloadCoolTimer = kReloadCoolTime;
        }
    }
}
