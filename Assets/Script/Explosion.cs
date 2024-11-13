using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int scaleSpeed = 6; // �g�傷�鑬�x
    private int maxScale = 7;    // �ő�X�P�[��
    private MeshRenderer objRenderer; // �I�u�W�F�N�g��Renderer
    private Color color;// �����̐F
    private CameraManager cameraManager;

    void Start()
    {
        cameraManager = Camera.main.GetComponent<CameraManager>();
        objRenderer = GetComponent<MeshRenderer>();
        // �����̐F��ۑ�
        if (objRenderer != null)
        {
            color = objRenderer.material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScaleUp();
    }

    // �X�P�[�������X�Ɋg�傷��֐�
    protected virtual void ScaleUp()
    {
        Vector3 newScale = transform.localScale;

        // �g�傷��
        if (newScale.x < maxScale) newScale.x += scaleSpeed * Time.deltaTime;
        if (newScale.y < maxScale) newScale.y += scaleSpeed * Time.deltaTime;
        if (newScale.z < maxScale) newScale.z += scaleSpeed * Time.deltaTime;

        // �X�V���ꂽ�X�P�[���̓K�p
        transform.localScale = newScale;

        // �A���t�@�l������������
        if (objRenderer != null)
        {
            Color currentColor = objRenderer.material.color;
            float alpha = Mathf.Lerp(1.0f, 0.0f, (newScale.x / maxScale));
            currentColor.a = alpha;
            objRenderer.material.color = currentColor;
        }

        // �X�P�[�����ő�l�𒴂�����폜
        if (newScale.x >= maxScale && newScale.y >= maxScale && newScale.z >= maxScale)
        {
            Destroy(gameObject);
        }

        cameraManager.StartShake(0.1f, 0.1f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("HitWall");
            collision.enabled = false;
            Debug.Log(this.transform.position);
            return;
        }
        Debug.Log("notHitWall");

        Vector3 hitPos = collision.transform.position;
        Vector3 direction = (hitPos - transform.position).normalized;

        Ray ray=new Ray(transform.position, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            Debug.Log(hit.collider.name);

            

            if (collision.gameObject.CompareTag("Bom"))
            {
                Debug.Log("���e�ɓ�������");
                Instantiate(this.gameObject, hit.transform.position, Quaternion.identity);
                Destroy(hit.collider.gameObject);
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EnemyControl>().ReduceHP(5);
            }

        }
        Debug.DrawRay(transform.position, direction);

    }
}
