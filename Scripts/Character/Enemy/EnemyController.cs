using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float moveRotationAngle = 25f;

    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;


    [Header("---- FIRE ----")]

    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;

    [SerializeField] float minFireInterval = 0.1f;
    [SerializeField] float maxFireInterval = 0.5f;


    private void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();    //关闭所有携程
    }

    /// <summary>
    /// 控制敌人随机移动的携程
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.RandomEnemyHalfRightPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if not arrive targetPosition 
            if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon) 
            {
                //keep moving targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                //make enemy rotation with X axis while moving
                transform.rotation = Quaternion.AngleAxis(
                    (targetPosition - transform.position).normalized.y * moveRotationAngle,
                    Vector3.right);
            }
            else
            {
                //get a new targetPosition
                targetPosition = Viewport.Instance.RandomEnemyHalfRightPosition(paddingX, paddingY);
            }

            yield return null;

        }
    }

    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            foreach(var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }

}
