using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleten<EnemyManager>
{
    [SerializeField] bool isSpawnEnemy = true;

    [SerializeField] GameObject[] enemyPrefabs;

    /// <summary>
    /// �������ɼ��ʱ��
    /// </summary>
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float timeBetweenWaves = 1f;

    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;

    /// <summary>
    /// �������е��˵��б�
    /// </summary>
    List<GameObject> enemyList;

    /// <summary>
    /// ���˲���
    /// </summary>
    int waveNumber = 1;
    /// <summary>
    /// ��������
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
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);   //������ķ���������ʽ�����Ͳ�������������һ��������

    }

    //bool NoEnemy() => enemyList.Count == 0;

    //��Start���������Я�̣��ﵽ��Ϸһ��ʼ��ִ��Я�̵�Ŀ��
    IEnumerator Start()
    {
        while (isSpawnEnemy)
        {
            yield return waitUntilNoEnemy;  //�������л��е���ʱ�� enemyList.Count == 0 ����Ϊfalseʱ������ȴ��� ��ִ֮������Ĵ���
            yield return waitTimeBetweenWaves;  //����һ����������֮ǰ����ȴ�һ��ʱ��
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
       

    }

    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);
        for (int i = 0; i < enemyAmount; i++)
        {
            ////����Ĵӵ��������л�ȡ��һ������
            //var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            //PoolManager.Release(enemy);

            //ÿ����һ�����˾ͽ������з������
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

            yield return waitTiemBetweenSpawns;

        }
        //��һ������ȫ��������֮���õ��˲���(waveNumber)��һ
        waveNumber++;
    }

    /// <summary>
    /// �Ƴ��б��е�ָ��Ԫ��
    /// </summary>
    /// <param name="enemy">��Ҫɾ����Ԫ��</param>
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
    //{
    //    enemyList.Remove(enemy);
    //}
}

