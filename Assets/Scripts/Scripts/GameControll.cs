using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GameControll : SingletonMono<GameControll>
{
    //存储每个星星预制体
    public List<GameObject> starPrefabs;
    //数组存储所有星星
    public Star[,] allStarArray;
    //星星最开始生成位置，也就是棋盘左下角星星位置
    private Vector3 mStarCreatBasePosition;
    //棋盘位置 星星父对象容器
    public Transform startContent;
    //星星生成位置
    private Vector3 mStartPositon;
    //星星移动速度
    public float moveSpeed = 3f;
    //检测相关
    //检测后要消除的星星
    public List<Star> destoryStarList;
    //游戏结束星星列表
    public List<Star> overGameList;
    // 射线检测的距离设定
    public float checkDistance = 1.0f;
    //点击星星的collider
    //Collider2D ownCollider;
    //检测方位
    private Vector3[] mDerectionList = { Vector3.up * 59, Vector3.down * 59, Vector3.left * 59, Vector3.right * 59 };
    //星星list元素是满的吗
    public bool isFull = true;
    //是否需要清除所有星星
    public bool isClear = false;
    //奖励分数
    public int awardScore;
    //得分相关
    // 每消除一个星星的基础分
    public int baseScore = 10;
    // 多消除星星个数的额外分数
    public int extraScoreFactor = 2;
    //cavans缩放因子
    private float scaleFactor;
    //阻挡左移时点击的图片
    private GameObject image;

    private int columeCount;

    public bool isStartGame;//是否开始游戏

    protected override void Awake()
    {
        base.Awake();
        //初始化存储所有星星的容器
        allStarArray = new Star[GameDataManager.row, GameDataManager.colume];
        //初始化游戏结束时检测的容器
        overGameList = new List<Star>();

        columeCount = GameDataManager.colume;
    }
    void Start()
    {
        //加载摄像机
        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/Camera", (a) => { });
        //加载游戏面板Canvas
        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/GameCanvas", (a) =>
        {
            scaleFactor = a.GetComponent<Canvas>().scaleFactor;
            //找到游戏面板的位置作为星星创建的父物体
            startContent = GameObject.Find("GamePanel").transform;
            //创建星星的初始位置
            //mStarCreatBasePosition = GameObject.Find("CreatPoint").transform.localPosition;
            mStarCreatBasePosition = new Vector3(-486, -906, 0);
            //创建星星
            //Invoke("CreatStar",1f);
            UIManager.Instance.canvasTrans = startContent.parent;
            //显示开始面板
            UIManager.Instance.ShowPanel<StartPanel>();
            //显示登录面板
            UIManager.Instance.ShowPanel<LoginPanel>();
    
            image = FindObjectOfType<GamePanel>().transform.Find("Image").gameObject;
            image.SetActive(false);
            //播放背景音乐
            MusicMgr.GetInstance().PlayBkMusic("Bk");
            CreatStar();
        });
    }

    void Update()
    {

        //如果清除标记为真，清除游戏界面所有星星
        if (isClear)
        {
            isClear = false;
            isFull = true;
            //startContent.GetComponent<GamePanel>().GameTime = 100;
            Invoke("GameOverAfter", 3f);
        }
    }
    /// <summary>
    /// 创建星星
    /// </summary>
    public void CreatStar()
    {

        for (int i = 0; i < GameDataManager.row; i++)
        {
            for (int j = 0; j < GameDataManager.colume; j++)
            {
                //计算第i行第j列的标准位置
                mStartPositon = mStarCreatBasePosition + 108 * new Vector3(j, i, 0);

                //实例化方块

                GameObject StarObj = Instantiate(starPrefabs[Random.Range(0, 4)], startContent.GetChild(8));

                allStarArray[i, j] = StarObj.GetComponent<Star>();
                //设置方块的位置
                allStarArray[i, j].GetComponent<RectTransform>().anchoredPosition3D = mStartPositon;
                //设置行列索引
                allStarArray[i, j].rowIndex = i;
                allStarArray[i, j].columnIndex = j;

            }
        }
        isFull = true;
    }
    /// <summary>
    /// 射线检测星星上下左右位置是否有相同颜色星星
    /// </summary>
    public void RaycastStar(Star onclickStar)
    {
        Transform StarTransform = onclickStar.transform;
        foreach (var direction in mDerectionList)
        {
            Debug.DrawRay(StarTransform.position + direction, direction * checkDistance, Color.red, 5.0f); // 用以可视化
            RaycastHit2D hit = Physics2D.Raycast(StarTransform.position + direction, direction, checkDistance, 1 << LayerMask.NameToLayer("Star"));
            if (hit.collider != null)
            {
                Star hitStar = hit.collider.GetComponent<Star>();
                if (onclickStar.color == hitStar.color && !destoryStarList.Contains(hitStar))
                {
                    destoryStarList.Add(hitStar);
                    RaycastStar(hitStar);
                }
            }
        }
    }

    /// <summary>
    /// 删除删除列表的星星
    /// </summary>
    public void DestroyDestroyStar()
    {
        if (destoryStarList.Count >= 2)
        {
            foreach (var item in destoryStarList)
            {
                Destroy(item.gameObject);
                allStarArray[item.rowIndex, item.columnIndex] = null;
            }
            isFull = false;
            GetScore(destoryStarList);
          DropStars();
        //    ShiftStarsLeft();
            if (IsLevelOver())
            {
                //print("关卡结束");
                isClear = true;
            }
        }

        destoryStarList.Clear();

    }
    //场景中是否还有可以消除的星星
    bool CanEliminateStars()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                if (allStarArray[y, x] != null) // 如果当前位置有星星
                {
                    // 检查当前星星上下左右四个方向是否有相同颜色的星星
                    if (HasAdjacentSameColorStar(y, x))
                    {
                        return true; // 存在可以消除的星星
                    }
                }
            }
        }

        return false; // 所有星星都无法消除
    }
    /// <summary>
    /// 上下左右是否有相同颜色星星
    /// </summary>
    bool HasAdjacentSameColorStar(int x, int y)
    {
        // 检查上方
        if (y + 1 < 10 && allStarArray[x, y + 1] && allStarArray[x, y].color == allStarArray[x, y + 1].color)
        {
            return true;
        }
        // 检查下方
        if (y - 1 > 0 && allStarArray[x, y - 1] && allStarArray[x, y].color == allStarArray[x, y - 1].color)
        {
            return true;
        }
        // 检查左侧
        if (x - 1 > 0 && allStarArray[x - 1, y] && allStarArray[x, y].color == allStarArray[x - 1, y].color)
        {
            return true;
        }
        // 检查右侧
        if (x + 1 < 12 && allStarArray[x + 1, y] && allStarArray[x, y].color == allStarArray[x + 1, y].color)
        {
            return true;
        }

        return false; // 四个方向都没有相同颜色的星星
    }
    /// <summary>
    /// 当前关卡是否结束
    /// </summary>
    public bool IsLevelOver()
    {
        if (isFull)
        {
            return false;
        }
        return !CanEliminateStars();
    }
    int StartCoroutineCount;  //开启的协程数
    /// <summary>
    /// 下落星星
    /// </summary>
    public void DropStars()
    {
        for (int y = 0; y < GameDataManager.colume; y++)
        {
            for (int x = 0; x < GameDataManager.row; x++)
            {
                //如果有元素为空
                if (allStarArray[x, y] == null)
                {
                    int moveCount = 0;
                    //遍历x列空元素上面的所有元素 找到一个不为空的
                    for (int above = x + 1; above < GameDataManager.row; above++)
                    {
                        ++moveCount;
                        //如果不为空
                        if (allStarArray[above, y] != null)
                        {
                            //向下移动一格
                            //  Star gameObject1 = allStarArray[above, y];


                            allStarArray[x, y] = allStarArray[above, y];
                            // allStarArray[x, y].transform.position += Vector3.down * moveCount * 108*scaleFactor;
                            allStarArray[x, y].gameObject.transform.DOMove(allStarArray[x, y].transform.position + Vector3.down * moveCount * 108 * scaleFactor, 0.1f);
          
                          //  StartCoroutine(Move(allStarArray[x, y].transform, allStarArray[x, y].transform.position + Vector3.down * moveCount * 108 * scaleFactor));
                            allStarArray[x, y].rowIndex = x;
                            allStarArray[x, y].columnIndex = y;
                            allStarArray[above, y] = null;
                            break;
                        }
                        if (above == GameDataManager.row - 1)
                        {
                           StartCoroutine(Move(null,Vector3.zero, true));
                        }
                    }
                }
            }
        }

    }

    IEnumerator Move(Transform currentPos, Vector3 targetPos, bool last = false)
    {
        
            yield return new WaitForSeconds(0.3f);
             ShiftStarsLeft();
        
    }


    /// <summary>
    /// 如果一列为空，左移星星
    /// </summary>
    public void ShiftStarsLeft()
    {
        bool[] Emptys = new bool[columeCount];
        for (int y = 0;  y < GameDataManager.colume; y++)
      // for (int y = GameDataManager.colume-1; y>=0&& y < GameDataManager.colume; y--)
            {
            if (IsColumnEmpty(y))
            {
               Emptys[y] = true;
            }
        }
        StopAllCoroutines();
       StartCoroutine( MoveColume(Emptys));
    }


    public IEnumerator MoveColume(bool[] Emptys)
    {
   
        for (int y = Emptys.Length-1;y>=0&& y < Emptys.Length; y--)
        {
            if (Emptys[y])
            {
                //循环空列右边的,将x值付给空的这列的右边那一列
                for (int x = y + 1; x < GameDataManager.colume; x++)
                {
                    for (int yy = 0; yy < GameDataManager.row; yy++)
                    {
                        if (allStarArray[yy, x] != null)
                        {
                            allStarArray[yy, x - 1] = allStarArray[yy, x];
                            allStarArray[yy, x - 1].gameObject.transform.DOMove(allStarArray[yy, x - 1].transform.position + Vector3.left * 108 * scaleFactor, 0.1f).OnStart(()=> { 
                                
                                  image.SetActive(true);
                            });
                            allStarArray[yy, x - 1].rowIndex = yy;
                            allStarArray[yy, x - 1].columnIndex = x - 1;
                            allStarArray[yy, x] = null;
                        }
                    }
                }
                yield return new WaitForSeconds(0.1f); //0.15
            }
            else
            {
                continue;
            }
        }
        image.SetActive(false);

    }

    //是否有空的一列
    public bool IsColumnEmpty(int y)
    {
        for (int x = 0; x < GameDataManager.row; x++)
        {
            if (allStarArray[x, y] != null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 游戏是否结束
    /// </summary>
    public bool IsGameOver()
    {
        //如果星星列表是满的，返回false
        if (isFull)
        {
            return false;
        }

        foreach (var item in allStarArray)
        {
            if (item != null)
            {
                RaycastStarOverGame(item);
            }
        }

        if (overGameList.Count >= 2)
        {
            overGameList.Clear();
            return false;
        }
        else
        {
            overGameList.Clear();
            return true;
        }

    }
    /// <summary>
    /// 射线检测剩下的星星是否还有能消除的（未使用该函数）
    /// </summary>
    public void RaycastStarOverGame(Star onclickStar)
    {
        Transform StarTransform = onclickStar.transform;
        foreach (var direction in mDerectionList)
        {
            RaycastHit2D hit = Physics2D.Raycast(StarTransform.position + direction, direction, checkDistance, 1 << LayerMask.NameToLayer("Star"));
            if (hit.collider != null)
            {
                Star hitStar = hit.collider.GetComponent<Star>();
                if (onclickStar.color == hitStar.color && !overGameList.Contains(hitStar))
                {
                    overGameList.Add(hitStar);
                    RaycastStarOverGame(hitStar);
                }
            }
        }
    }


    //删除星星得到分数
    public void GetScore(List<Star> stars)
    {
        int score = 0;
        for (int i = 0; i < stars.Count; i++)
        {
            score += baseScore + (i * extraScoreFactor);
        }
        GameDataManager.CurrentScore += score;
    }
    //游戏关卡结束之后
    public void GameOverAfter()
    {
        int i = 0;

        //yield return new WaitForSeconds(2f);
        foreach (var item in allStarArray)
        {
            if (item != null)
            {
                ++i;
                Destroy(item.gameObject);
            }
        }
        if (i < 10)
        {
            awardScore = 2000 - i * 100;
            GameDataManager.remainStarScore = awardScore;
        }
        else
        {
            GameDataManager.remainStarScore = 0;
        }

        var game = UIManager.Instance.GetPanel<GamePanel>();
        game.remainStarScore.gameObject.SetActive(true);
        //yield return new WaitForSeconds(2f);
        Invoke("CloseRemainStarScore", 2f);
        GameDataManager.CurrentScore += awardScore;
        Invoke("GameOverShow", 4f);
        awardScore = 0;
    }

    /// <summary>
    /// 隐藏通关文字
    /// </summary>
    public void CloseWinText()
    {
        UIManager.Instance.HidePanel<EndPanel>();
        startContent.GetComponent<GamePanel>().GameTime = 100;
        CreatStar();
    }
    /// <summary>
    /// 隐藏奖励文字
    /// </summary>
    public void CloseRemainStarScore()
    {
        var game = UIManager.Instance.GetPanel<GamePanel>();
        game.remainStarScore.gameObject.SetActive(false);
    }
    /// <summary>
    /// 游戏结束后显示
    /// </summary>
    public void GameOverShow()
    {
        if (GameDataManager.CurrentScore > GameDataManager.TargetScore)
        {
            GameDataManager.CurrentLevel++;
            GameDataManager.TargetScore += 2500;
            //print("分数大于目标分数");
            UIManager.Instance.ShowPanel<EndPanel>();
            startContent.GetComponent<GamePanel>().GameTime = 100;
            Invoke("CloseWinText", 2f);
        }
        else
        {
            //显示失败界面
            UIManager.Instance.ShowPanel<EndPanel>();
        }
    }
}
