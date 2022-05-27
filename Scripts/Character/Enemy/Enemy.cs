using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int dieEnergyBonus = 3;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }

    public override void Die()
    {
        //�ڵ���������һ�õ÷�
        ScoreManager.Instance.AddScore(scorePoint);
        //�ڵ��������������dieEnergyBonus ��ֵ������
        PlayerEnergy.Instance.Obtian(dieEnergyBonus);
        //�ڵ����������ö�����б����Ƴ�
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}
