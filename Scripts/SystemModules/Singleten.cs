using UnityEngine;

/// <summary>
/// 这是一个用来实现单例模式的一个类
/// </summary>
public class Singleten<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
