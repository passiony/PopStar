using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public Button Startbutton;

    public Button TestBtn;

    public override void Init()
    {
        Startbutton.onClick.AddListener(()=> 
        {
            GameControll.GetInstance().isStartGame = true;
            UIManager.Instance.HidePanel<StartPanel>();
        });
        TestBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<TestPanel>();
        });
    }

    
}
