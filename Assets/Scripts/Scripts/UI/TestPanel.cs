using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BasePanel
{
    public Button CloseBtn;
    public override void Init()
    {
        CloseBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TestPanel>();
        });
    }

}
