using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//由于Pool类没有继承 MonoBehaviour 类 从而使用[System.Serializable] 将其在编辑器里面曝露出来
[System.Serializable]public class Pool 
{
    //public GameObject Prefab 
    //{
    //    get
    //    {
    //        return prefab;
    //    } 
    //}
    public GameObject Prefab => prefab;
    public int Size => size;
    public int runTimeSize => queue.Count;

    [SerializeField] GameObject prefab;

    /// <summary>
    /// 记录对象池的尺寸（预制体的个数）
    /// </summary>
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    /// <summary>
    /// 池对象的父对像
    /// </summary>
    Transform parent;

    /// <summary>
    /// 初始化池，生成一定数量的对象
    /// </summary>
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;   //将传入的TransForm 设置为池对象的父级对象

        //让所有预先生成的对象入列
        for(int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    
    /// <summary>
    /// 启用可用的对象
    /// </summary>
    /// <returns>一个可用并被启用的对象</returns>
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    /// <summary>
    /// 启用可用的对象,并指定它的生成位置
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <returns>一个可用并被启用的对象</returns>
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    /// <summary>
    /// 启用可用的对象、 指定它的生成位置 并且 指定它的旋转角度
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">旋转角度</param>
    /// <returns>一个可用并被启用的对象</returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    /// <summary>
    /// 启用可用的对象、 指定它的生成位置、 指定它的旋转角度 并且 指定它的本地缩放大小
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">旋转角度</param>
    /// <param name="localScale">本地缩放大小</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }

    ///// <summary>
    ///// 让使用完的对象返回队列(返回对象池）
    ///// </summary>
    ///// <param name="gameObject"></param>
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}

    /// <summary>
    /// 取出队列中可用的对象
    /// </summary>
    /// <returns>一个池中的可用对象</returns>
    private GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)    //当队列元素大于零 且 第一个元素没有正在被启用的情况下可以直接调用一个可用对象
        {
            availableObject = queue.Dequeue();
        }
        else    //当队列中没有可用对象了 就临时生成一个来取出
        {
            availableObject = Copy();
        }

        //让可用的对象提前返回池中
        queue.Enqueue(availableObject);

        return availableObject;
    }

    /// <summary>
    /// 用该函数来生成预制体的复制体，对象默认为禁用
    /// </summary>
    /// <returns>一个复制了的预制体</returns>
    private GameObject Copy()
    {
        //生成备用对象，并禁用它们
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }
}
