using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : BasePanel
{
    //���¿�ʼ��ť
    public Button reStartBtn;
    //����ʼ��
    public override void Init()
    {
        reStartBtn.onClick.AddListener(() =>
        {
            GameDataManager.CurrentLevel = 1;
            GameDataManager.CurrentScore = 0;
            GameDataManager.TargetScore = 1000;
            GameControll.GetInstance().startContent.GetComponent<GamePanel>().GameTime = 100;
            GameControll.GetInstance().CreatStar();
            UIManager.Instance.HidePanel<EndPanel>();
        });
    }

    
}
