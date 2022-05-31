using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    private protected virtual void Awake()
    {
        //由于Trail 组件是挂在子物体上的 因此要用GetComponentInChildren 来获取到它的组件
        trail = GetComponentInChildren<TrailRenderer>();

        if (moveDirection != Vector2.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }
    }

    private void OnDisable()
    {
        //清理轨迹
        if (trail == null) return;
        trail.Clear();


    }

    private protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        //实现每次玩家子弹命中后能量增加一个PlayerEnergy.PERCENT 的值
        PlayerEnergy.Instance.Obtian(PlayerEnergy.PERCENT);
    }

}


