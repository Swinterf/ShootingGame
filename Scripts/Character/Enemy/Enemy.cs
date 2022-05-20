using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int dieEnergyBonus = 3;

    public override void Die()
    {
        //�ڵ��������������dieEnergyBonus ��ֵ������
        PlayerEnergy.Instance.Obtian(dieEnergyBonus);
        //�ڵ����������ö�����б����Ƴ�
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}
