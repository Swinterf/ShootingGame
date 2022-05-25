using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        base.OnEnable();
    }
}
