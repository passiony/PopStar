using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Text CurrentLevel;
    public Text TargetScore;
    public Text CurrentScore;
    public Button SettingBtn;
    public Text remainStarScore;

    public float GameTime;
    public Text GameTimeTxt;

    public bool gameStart;
    public UnityEvent OnGameEnd;

    protected override void Awake()
    {
        base.Awake();
        remainStarScore.gameObject.SetActive(false);
    }

    public override void Init()
    {
        SettingBtn.onClick.AddListener(() =>
        {
            //��ʾ��ͣ���
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
    }

    protected override void Update()
    {
        base.Update();
        if (gameStart)
        {
            if (GameTime >= 0)
            {
                GameTime -= Time.deltaTime;
                GameTimeTxt.text = "Time:" + ((int)GameTime);
            }

            if (GameTime <= 0)
            {
                gameStart = false;
                OnGameEnd?.Invoke();
            }
        }

        // if (GameControll.GetInstance().isStartGame)
        // {
        //     getText();
        //     if (GameTime <= 0)
        //     {
        //        GameControll.GetInstance(). isClear = false;
        //         GameControll.GetInstance(). isFull = true;
        //
        //         GameControll.GetInstance().GameOverShow();
        //         return;
        //     }
        // }
    }

    void getText()
    {
        if (GameTime >= 0)
        {
            GameTime -= Time.deltaTime;
        }

        GameTimeTxt.text = "Time:" + ((int)GameTime).ToString();
        CurrentLevel.text = "�ؿ���" + GameDataManager.CurrentLevel.ToString();
        TargetScore.text = "Ŀ�꣺" + GameDataManager.TargetScore.ToString();
        CurrentScore.text = "������" + GameDataManager.CurrentScore.ToString();
        remainStarScore.text = "����" + GameDataManager.remainStarScore.ToString();
    }

    public void RefreshText(int level, int tScore, int cScore, int award)
    {
        GameTimeTxt.text = "Time:" + ((int)GameTime);
        CurrentLevel.text = "�ؿ�:" + level;
        TargetScore.text = "Ŀ��:" + tScore;
        CurrentScore.text = "����:" + cScore;
        remainStarScore.text = "����:" + award;
    }
}