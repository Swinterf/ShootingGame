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
            Move();
            yield return null;
        }
    }

    public void Move()  => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

    private protected void SetTarget(GameObject target) => this.target = target;
    //{
    //    this.target = target;
    //}

    private protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        //TryGetComponent<Character> ���� GetCpmponent<Character> ���ĵ����ܸ���
        if (collision.gameObject.TryGetComponent<Character>(out Character character))   //���ӵ�������Chara ��ʱ
        {
            character.TakeDamage(damage);

            //var contactPoint = collision.GetContact(0);     //��GetContact ����������ײʱ����ײ��   ������GetContact(0)��ʾ��һ����ײ��
            //PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            PoolManager.Release(hitVFX,
                collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFXs);   //����������Ч

            //����Ŀ����ӵ����ûص��������׼���ٴα�����
            gameObject.SetActive(false);
        }
    }
}
