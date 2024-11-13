using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun3 : Gun
{
    [SerializeField] private Grenade grenade;
    private float grenadeTimer = 0.5f;
    private float grenadeCount = 0;
    //[SerializeField] private GameObject firePosition;

    public override void Update()
    {
        grenadeCount += Time.deltaTime;
        base.Update();
    }

    public override void Shot()
    {
        //shotTimer = 0.2f;
        

        if (grenadeCount >= grenadeTimer)
        {
            Grenade gre = Instantiate(grenade, firePosition.transform.position, Quaternion.identity);

            gre.SetUp(firePosition.transform.position, firePosition.transform.rotation);

            grenadeCount = 0;
        }

        base.Shot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.enabled = false;
        }
    }
}
