using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /// <summary>
    /// 死亡特效
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
    /// 启用头顶血条的函数
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    /// <summary>
    /// 隐藏头顶血条的函数
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
            //更新血条状态
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
    /// 实现玩家回复血量的函数
    /// </summary>
    /// <param name="value"></param>
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;

        //health += value;
        //health = Mathf.Clamp(health, 0f, maxHealth);   //防止生命值溢出
        health = Mathf.Clamp(health + value, 0, maxHealth);

        if (isShowOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth);
        }
    }

    /// <summary>
    /// 实现持续恢复血量功能
    /// </summary>
    /// <param name="waitTime">恢复的间隔时间</param>
    /// <param name="percent">每次恢复生命占最大生命的百分比</param>
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
    /// 实现持续血量扣除的携程
    /// </summary>
    /// <param name="waitTime">扣血的间隔时间</param>
    /// <param name="percent">每次扣血占最大生命值的百分比</param>
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
