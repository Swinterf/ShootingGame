using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;

    [SerializeField] AudioData[] hitSFXs;

    [SerializeField] float damage = 1f;

    [SerializeField] float moveSpeed = 10f;

    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectlyCoroutine());
    }

    IEnumerator MoveDirectlyCoroutine()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        //TryGetComponent<Character> 比起 GetCpmponent<Character> 消耗的性能更少
        if (collision.gameObject.TryGetComponent<Character>(out Character character))   //当子弹命中了Chara 类时
        {
            character.TakeDamage(damage);

            //var contactPoint = collision.GetContact(0);     //用GetContact 返回两者碰撞时的碰撞点   其中用GetContact(0)表示第一个碰撞点
            //PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            PoolManager.Release(hitVFX,
                collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFXs);   //播放命中音效

            //命中目标后将子弹禁用回到对象池中准备再次被启动
            gameObject.SetActive(false);
        }
    }
}
