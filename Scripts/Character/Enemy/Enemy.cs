using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int dieEnergyBonus = 3;

    public override void Die()
    {
        //在敌人死亡后奖励玩家dieEnergyBonus 数值的能量
        PlayerEnergy.Instance.Obtian(dieEnergyBonus);
        //在敌人死亡后让对象从列表中移除
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}
