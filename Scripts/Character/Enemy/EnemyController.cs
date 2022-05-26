using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float moveRotationAngle = 25f;




    [Header("---- FIRE ----")]

    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;
    [SerializeField] AudioData[] projetcileLaunchSFXs; 

    [SerializeField] float minFireInterval = 0.1f;
    [SerializeField] float maxFireInterval = 0.5f;

    float paddingX;
    float paddingY;

    float maxFramDistance;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();    //�ر�����Я��
    }

    /// <summary>
    /// ���Ƶ�������ƶ���Я��
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.RandomEnemyHalfRightPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if not arrive targetPosition 
            if (Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime) 
            {
                //keep moving targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

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

            yield return waitForFixedUpdate;

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

            AudioManager.Instance.PlayRandomSFX(projetcileLaunchSFXs);  //���ſ�����Ч
        }
    }

}
