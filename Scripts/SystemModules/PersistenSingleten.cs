using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenSingleten<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    private protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }

        //确保在场景加载这个持久单例不会消失
        DontDestroyOnLoad(gameObject);
    }

}
