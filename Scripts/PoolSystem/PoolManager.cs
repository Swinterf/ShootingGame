using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] VFXPools;
    [SerializeField] Pool[] enemyPools;

    /// <summary>
    /// 该字典用于匹配游戏对象预制体 和 与之匹配的对象池
    /// </summary>
    static Dictionary<GameObject, Pool> dictionary;

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(VFXPools);
        Initialize(enemyPools);
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(VFXPools);
        CheckPoolSize(enemyPools);
    }
#endif

    private void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
#if UNITY_EDITOR
            //如果有错误操作导致有多个键（Prefab）对应一个值（pool） 则直接跳过本次循环不再初始化一个新池
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("The same Prefab in multiple pool ! Prefab " + pool.Prefab.name);

                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool ：" + pool.Prefab.name).transform;
            
            //使得挂载脚本的物体成为每一个池对象的父对象
            poolParent.parent = transform;

            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// 检测初始对象池的大小是否合适
    /// </summary>
    /// <param name="pools">所有对象池</param>
    private void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if(pool.Size < pool.runTimeSize)
            {
                Debug.LogWarning(
                    string.Format("Prefab :{0} has a bigger runTimeSize {1} than Size {2}",
                    pool.Prefab.name,
                    pool.runTimeSize,
                    pool.Size));
            }
        }
    }

    /// <summary>
    /// 释放一个池中准备好被启用的预制体
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <returns>一个可用的池中对象</returns>
    static public GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager can not find the prefab :" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// 释放一个池中准备好被启用的预制体 并且指定其生成位置
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <param name="position">生成位置</param>
    /// <returns>一个可用的池中对象</returns>
    static public GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager can not find the prefab :" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// 释放一个池中准备好被启用的预制体 、指定其生成位置 并且 指定其旋转角度
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">旋转角度</param>
    /// <returns>一个可用的池中对象</returns>
    static public GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager can not find the prefab :" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    /// <summary>
    /// 释放一个池中准备好被启用的预制体 、指定其生成位置 、指定其旋转角度 并且指定其本地缩放大小
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">旋转角度</param>
    /// <param name="localScale">本地缩放大小</param>
    /// <returns>一个可用的池中对象</returns>
    static public GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager can not find the prefab :" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }


}
