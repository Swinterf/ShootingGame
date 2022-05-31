using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    private protected virtual void Awake()
    {
        //����Trail ����ǹ����������ϵ� ���Ҫ��GetComponentInChildren ����ȡ���������
        trail = GetComponentInChildren<TrailRenderer>();

        if (moveDirection != Vector2.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
    }

    private void OnDisable()
    {
        //����켣
        if (trail == null) return;
        trail.Clear();


    }

    private protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        //ʵ��ÿ������ӵ����к���������һ��PlayerEnergy.PERCENT ��ֵ
        PlayerEnergy.Instance.Obtian(PlayerEnergy.PERCENT);
    }

}


