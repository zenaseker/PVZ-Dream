using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MianLevelInformation : MonoBehaviour
{
    public MianLevel MianLevelID;
    public virtual void OnEnable()
    {
        this.transform.GetChild(2).gameObject.SetActive(Attribute.Instance.filedInfo.MianFinishLevel > MianLevelID.ID);
    }
    public void OnClick()
    {
        ShowLevelInfo.Instance.Init(this);
    }
}

[System.Serializable]
public class MianLevel
{
    public int ID = -1;
    public string Name = "";
    [TextArea(0,4)]
    public string Text = "";
    [TextArea(0, 3)]
    public string Info = "";
    [TextArea(0, 2)]
    public string ReWard = "";
}