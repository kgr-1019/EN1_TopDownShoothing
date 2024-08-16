using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject : MonoBehaviour
{
    public GameObject gameObject;
    public GameObject target;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            transform.SetParent(gameObject.transform);
        }
    }
}
