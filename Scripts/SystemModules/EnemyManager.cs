using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleten<EnemyManager>
{
    [SerializeField] bool isSpawnEnemy = true;

    [SerializeField] GameObject[] enemyPrefabs;

    /// <summary>
    /// 敌人生成间隔时间
    /// </summary>
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float timeBetweenWaves = 1f;

    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;

    /// <summary>
    /// 管理所有敌人的列表
    /// </summary>
    List<GameObject> enemyList;

    /// <summary>
    /// 敌人波数
    /// </summary>
    int waveNumber = 1;
    /// <summary>
    /// 敌人数量
    /// </summary>
    int enemyAmount;

    WaitForSeconds waitTiemBetweenSpawns;
    WaitForSeconds waitTimeBetweenWaves;

    WaitUntil waitUntilNoEnemy;


    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTiemBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        //waitUntilNoEnemy = new WaitUntil(NoEnemy);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);   //传入拉姆达匿名表达式这样就不用再重新申明一个函数了

    }

    //bool NoEnemy() => enemyList.Count == 0;

    //将Start函数改造成携程，达到游戏一开始就执行携程的目的
    IEnumerator Start()
    {
        while (isSpawnEnemy)
        {
            yield return waitUntilNoEnemy;  //当场景中还有敌人时（ enemyList.Count == 0 返回为false时）挂起等待， 反之执行下面的代码
            yield return waitTimeBetweenWaves;  //在下一波敌人生成之前挂起等待一段时间
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
       

    }

    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);
        for (int i = 0; i < enemyAmount; i++)
        {
            ////随机的从敌人数组中获取到一个敌人
            //var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            //PoolManager.Release(enemy);

            //每生成一个敌人就将其入列方便管理
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

            yield return waitTiemBetweenSpawns;

        }
        //当一波敌人全部生成完之后让敌人波数(waveNumber)加一
        waveNumber++;
    }

    /// <summary>
    /// 移除列表中的指定元素
    /// </summary>
    /// <param name="enemy">想要删除的元素</param>
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
    //{
    //    enemyList.Remove(enemy);
    //}
}

