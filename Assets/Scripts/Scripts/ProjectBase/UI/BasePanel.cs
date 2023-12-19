using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //专门用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 10;

    //当前是隐藏还是显示
    public bool isShow = false;

    //当面板隐藏后,想要做的事情
    private UnityAction hideCallBack = null;

    //之所以将函数设置为虚函数 是为了让子类也可以重写这两个函数 有可能子类也需要在这两个函数里写逻辑
    protected virtual void Awake()
    {
        //如果忘记添加canvasGroup脚本就直接添加一个
        if (canvasGroup == null)
        {
            canvasGroup = transform.gameObject.AddComponent<CanvasGroup>();
        }
        //一开始就获取面板上挂载的组件
        canvasGroup = GetComponent<CanvasGroup>();

    }

    protected virtual void Start()
    {
        // Init();
    }

    /// <summary>
    /// 注册控件事件的方法 所有的子面板 都要去注册一些控件事件
    /// 所以写成抽象方法 子类必须实现
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 显示自己时的逻辑
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;

        isShow = true;
    }

    /// <summary>
    /// 隐藏自己时的逻辑
    /// </summary>
    /// <param name="callback">面板隐藏后想要做的逻辑</param>
    public virtual void HideMe(UnityAction callback)
    {
        canvasGroup.alpha = 1;

        isShow = false;

        hideCallBack = callback;
    }

    protected virtual void Update()
    {
        //当处于显示状态时,当alpha不为1,就进入一直加到1 到1之后就停止增加
        //淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //淡出
        if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //面板隐藏后 执行的逻辑
                hideCallBack?.Invoke();
            }

        }
    }
}