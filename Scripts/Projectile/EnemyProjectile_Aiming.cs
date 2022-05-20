using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    /// <summary>
    /// ���ڸ���ֵ�Ĳ���ȷ�Ӷ�������Ϸһ��ʼ������㽫��úܲ�׼ȷ��
    /// �������Я�����ȵȴ�һ֡��ʱ�䱣֤�ӵ����ٷ����ȷ����
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;
        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}