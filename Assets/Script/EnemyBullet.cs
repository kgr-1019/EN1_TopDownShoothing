using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void Move()
    {
        moveTime = 0.3f;
        base.Move();
    }
}
