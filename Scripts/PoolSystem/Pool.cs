using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Pool��û�м̳� MonoBehaviour �� �Ӷ�ʹ��[System.Serializable] �����ڱ༭��������¶����
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
    /// ��¼����صĳߴ磨Ԥ����ĸ�����
    /// </summary>
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    /// <summary>
    /// �ض���ĸ�����
    /// </summary>
    Transform parent;

    /// <summary>
    /// ��ʼ���أ�����һ�������Ķ���
    /// </summary>
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;   //�������TransForm ����Ϊ�ض���ĸ�������

        //������Ԥ�����ɵĶ�������
        for(int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    
    /// <summary>
    /// ���ÿ��õĶ���
    /// </summary>
    /// <returns>һ�����ò������õĶ���</returns>
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    /// <summary>
    /// ���ÿ��õĶ���,��ָ����������λ��
    /// </summary>
    /// <param name="position">����λ��</param>
    /// <returns>һ�����ò������õĶ���</returns>
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    /// <summary>
    /// ���ÿ��õĶ��� ָ����������λ�� ���� ָ��������ת�Ƕ�
    /// </summary>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">��ת�Ƕ�</param>
    /// <returns>һ�����ò������õĶ���</returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    /// <summary>
    /// ���ÿ��õĶ��� ָ����������λ�á� ָ��������ת�Ƕ� ���� ָ�����ı������Ŵ�С
    /// </summary>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">��ת�Ƕ�</param>
    /// <param name="localScale">�������Ŵ�С</param>
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
    ///// ��ʹ����Ķ��󷵻ض���(���ض���أ�
    ///// </summary>
    ///// <param name="gameObject"></param>
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}

    /// <summary>
    /// ȡ�������п��õĶ���
    /// </summary>
    /// <returns>һ�����еĿ��ö���</returns>
    private GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)    //������Ԫ�ش����� �� ��һ��Ԫ��û�����ڱ����õ�����¿���ֱ�ӵ���һ�����ö���
        {
            availableObject = queue.Dequeue();
        }
        else    //��������û�п��ö����� ����ʱ����һ����ȡ��
        {
            availableObject = Copy();
        }

        //�ÿ��õĶ�����ǰ���س���
        queue.Enqueue(availableObject);

        return availableObject;
    }

    /// <summary>
    /// �øú���������Ԥ����ĸ����壬����Ĭ��Ϊ����
    /// </summary>
    /// <returns>һ�������˵�Ԥ����</returns>
    private GameObject Copy()
    {
        //���ɱ��ö��󣬲���������
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }
}
