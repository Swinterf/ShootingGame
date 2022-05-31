using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [Header("---- EXPLOSION ----")]
    [SerializeField] AudioData explosionSFX = null;
    [SerializeField] GameObject explosionVFX = null;

    [SerializeField] LayerMask enemyLayerMask = default;

    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float explosionDamage = 100f;

    [Header("---- SFX ----")]
    [SerializeField] AudioData targetAcquireVoice = null;

    [Header("---- SPEED CHANGE ----")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 15f;
    [SerializeField] float variableSpeedDelayTime = 0.5f;

    WaitForSeconds waitVariableSpeedDelayTime;

    private protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelayTime = new WaitForSeconds(variableSpeedDelayTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    private protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        //Spawn a explosion VFX
        PoolManager.Release(explosionVFX, transform.position);
        //Play a explosion SFX
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        //enemies in explosion take AOE damage
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeedDelayTime;

        moveSpeed = highSpeed;

        if(target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquireVoice);
        }
    }

}
