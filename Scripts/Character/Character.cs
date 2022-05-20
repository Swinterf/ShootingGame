using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /// <summary>
    /// ������Ч
    /// </summary>
    [SerializeField] GameObject deathVFX;


    [Header("---- HEALTH ----")]

    [SerializeField] private protected float maxHealth;
    [SerializeField] bool isShowOnHeadHealthBar = true;
    [SerializeField] StateBar onHeadHealthBar; 

    private protected float health;

    private protected virtual void OnEnable()
    {
        health = maxHealth;

        if (isShowOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }

    /// <summary>
    /// ����ͷ��Ѫ���ĺ���
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    /// <summary>
    /// ����ͷ��Ѫ���ĺ���
    /// </summary>
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (isShowOnHeadHealthBar && gameObject.activeSelf) 
        {
            //����Ѫ��״̬
            onHeadHealthBar.UpdateState(health, maxHealth);
        }

        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        health = 0;
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    } 

    /// <summary>
    /// ʵ����һظ�Ѫ���ĺ���
    /// </summary>
    /// <param name="value"></param>
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;

        //health += value;
        //health = Mathf.Clamp(health, 0f, maxHealth);   //��ֹ����ֵ���
        health = Mathf.Clamp(health + value, 0, maxHealth);

        if (isShowOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth);
        }
    }

    /// <summary>
    /// ʵ�ֳ����ָ�Ѫ������
    /// </summary>
    /// <param name="waitTime">�ָ��ļ��ʱ��</param>
    /// <param name="percent">ÿ�λָ�����ռ��������İٷֱ�</param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while(health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth * percent);
        }
    }

    /// <summary>
    /// ʵ�ֳ���Ѫ���۳���Я��
    /// </summary>
    /// <param name="waitTime">��Ѫ�ļ��ʱ��</param>
    /// <param name="percent">ÿ�ο�Ѫռ�������ֵ�İٷֱ�</param>
    /// <returns></returns>
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }
}
