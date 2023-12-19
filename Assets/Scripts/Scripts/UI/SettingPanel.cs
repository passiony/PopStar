using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button CloseBtn;

    public Toggle BkMusic;

    public Slider BkValue;
    public override void Init()
    {
        CloseBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SettingPanel>();
        });
        BkMusic.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                MusicMgr.GetInstance().PlayBkMusic("Bk");
            }
            else
            {
                MusicMgr.GetInstance().StopBKMusic();      
            }
        });
        BkValue.onValueChanged.AddListener((value) =>
        {
            MusicMgr.GetInstance().ChangeBKValue(value);
        });
    }

    
}
