using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public List<Door> connectedDoors; // ボタンが押されたかのフラグ
    //public bool isPressed = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
