using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏状态的枚举
public enum GameState
{
    Load,
    Playing,
    Win,
    Defeat
}
public class GameDataManager 
{

    //棋盘的大小 行列数
    public static int row = 12;
    public static int colume = 10;
    //当前关卡
    public static int CurrentLevel=1;
    //当前分数
    public static int CurrentScore = 0;
    //当前目标分数
    public static int TargetScore = 1000;
    //历史最高分数
    public static int maxGameScore = 0;
    //当前的游戏状态
    public static GameState gameState = GameState.Load;
    //剩余星星得分
    public static int remainStarScore;
    
}
