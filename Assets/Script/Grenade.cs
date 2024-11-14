using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private float speed = 8.0f;
    private Vector3 direction;
    [SerializeField] private Explosion explosion;
    private CameraManager cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
        rb.rotation = Quaternion.LookRotation(direction);
    }

    public void SetUp(Vector3 firePosition, Quaternion rotation)
    {
        direction = rotation * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            //collision.gameObject.GetComponent<EnemyControl>().ReduceHP(5);
            explosion = Instantiate(explosion, transform.position, Quaternion.identity);
            cameraScript.StartShake(0.5f, 0.5f);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
            explosion = Instantiate(explosion, transform.position, Quaternion.identity);
            cameraScript.StartShake(0.5f, 0.5f);
        }
    }
}
