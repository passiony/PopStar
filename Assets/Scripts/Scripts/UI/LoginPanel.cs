using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    public InputField NameinputField;
    public InputField PassWoldinputField;

    public Button LoginBtn;
    public bool isName;
    public bool isPassWold;
    public override void Init()
    {
        NameinputField.onValueChanged.AddListener((value) =>
        {
            if (value!=null)
            {
                isName = true;
            }
        });

        PassWoldinputField.onValueChanged.AddListener((value) => 
        {
            if (value!=null)
            {
                isPassWold = true;
            }
        });
        LoginBtn.onClick.AddListener(() =>
        {
            if (isName&&isPassWold)
            {
                UIManager.Instance.HidePanel<LoginPanel>();
            }
            else
            {
                //UIManager.Instance.ShowPanel<>();
            }
        });
    }

    
}
