using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StarColorEnum
{
    Blue,Green,Orange,Purple,Red
}
public class Star : MonoBehaviour
{
    public int rowIndex;
    public int columnIndex;
    public StarColorEnum color;

    [HideInInspector]
    public Button button;
    //检测后要消除的星星
    public List<Star> aroundSameColorList;

    // 射线检测的距离设定
    public float checkDistance = 1.0f;

    private void Awake()
    {
        button = GetComponent<Button>();
        // button.onClick.AddListener(OnClick);
    }
    //
    // public void OnClick()
    // {
    //     GameControll.GetInstance().RaycastStar(this);
    //     GameControll.GetInstance().DestroyDestroyStar();
    // }
    
}
