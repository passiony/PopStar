using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单例mono基类
public class SingletonMono<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //继承了Mono的脚本 不能够直接new
        //只能通过拖动到对象上 或者 通过 加脚本的api AddComponent去加脚本
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
	
}
