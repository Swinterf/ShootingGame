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
    /// ���ֵ�����ƥ����Ϸ����Ԥ���� �� ��֮ƥ��Ķ����
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
            //����д�����������ж������Prefab����Ӧһ��ֵ��pool�� ��ֱ����������ѭ�����ٳ�ʼ��һ���³�
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("The same Prefab in multiple pool ! Prefab " + pool.Prefab.name);

                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool ��" + pool.Prefab.name).transform;
            
            //ʹ�ù��ؽű��������Ϊÿһ���ض���ĸ�����
            poolParent.parent = transform;

            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// ����ʼ����صĴ�С�Ƿ����
    /// </summary>
    /// <param name="pools">���ж����</param>
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
    /// �ͷ�һ������׼���ñ����õ�Ԥ����
    /// </summary>
    /// <param name="prefab">Ŀ��Ԥ����</param>
    /// <returns>һ�����õĳ��ж���</returns>
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
    /// �ͷ�һ������׼���ñ����õ�Ԥ���� ����ָ��������λ��
    /// </summary>
    /// <param name="prefab">Ŀ��Ԥ����</param>
    /// <param name="position">����λ��</param>
    /// <returns>һ�����õĳ��ж���</returns>
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
    /// �ͷ�һ������׼���ñ����õ�Ԥ���� ��ָ��������λ�� ���� ָ������ת�Ƕ�
    /// </summary>
    /// <param name="prefab">Ŀ��Ԥ����</param>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">��ת�Ƕ�</param>
    /// <returns>һ�����õĳ��ж���</returns>
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
    /// �ͷ�һ������׼���ñ����õ�Ԥ���� ��ָ��������λ�� ��ָ������ת�Ƕ� ����ָ���䱾�����Ŵ�С
    /// </summary>
    /// <param name="prefab">Ŀ��Ԥ����</param>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">��ת�Ƕ�</param>
    /// <param name="localScale">�������Ŵ�С</param>
    /// <returns>һ�����õĳ��ж���</returns>
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
