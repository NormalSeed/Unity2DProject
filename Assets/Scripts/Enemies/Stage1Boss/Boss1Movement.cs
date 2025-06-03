using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Movement : EnemyMovement
{
    protected override void Init()
    {
        base.Init();
        detectRange = 10f;
        attackRange = 3.5f;
    }
}
