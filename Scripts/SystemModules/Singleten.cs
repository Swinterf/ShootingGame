using UnityEngine;

/// <summary>
/// ����һ������ʵ�ֵ���ģʽ��һ����
/// </summary>
public class Singleten<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
