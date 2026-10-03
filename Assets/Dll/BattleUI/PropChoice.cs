using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropChoice : MonoBehaviour
{
    public List<Sprite> propuis;
    public GameObject buttonhelp1;
    public GameObject buttonhelp2;
    public GameObject buttonhelp3;
    public Image text1;
    public Image text2;
    public Image text3;
    List<Prop> PropList;

    public void OnEnable()
    {
        PropList = new List<Prop>();
        if (Attribute.Instance.filedInfo.MianFinishLevel >= 0)
        {
            PropList.Add(Prop.Shovel);
        }
        else
        {
            buttonhelp1.SetActive(false);
        }
        if (Attribute.Instance.filedInfo.MianFinishLevel >= 8)
        {
            PropList.Add(Prop.Hammer);
        }
        if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(100))
        {
            PropList.Add(Prop.Glove);
        }
        else
        {
            buttonhelp2.SetActive(false);
        }
        if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(200))
        {
            PropList.Add(Prop.WateringCan);
        }
        else
        {
            buttonhelp3.SetActive(false);
        }
        if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(300))
        {
            PropList.Add(Prop.Fertilize);
        }
        if (Attribute.Instance.filedInfo.SaveProps != null)
        {
            text1.sprite = propuis[(int)Attribute.Instance.filedInfo.SaveProps[0]];
            text2.sprite = propuis[(int)Attribute.Instance.filedInfo.SaveProps[1]];
            text3.sprite = propuis[(int)Attribute.Instance.filedInfo.SaveProps[2]];
        }
        else
        {
            text1.sprite = propuis[0];
            text2.sprite = propuis[0];
            text3.sprite = propuis[0];
        }
    }
    public void SaveProp(int Place)
    {
        if (Attribute.Instance.filedInfo.SaveProps == null)
        {
            Attribute.Instance.filedInfo.SaveProps = new Prop[3] { 0, 0, 0 };
        }
        int num = (int)Attribute.Instance.filedInfo.SaveProps[Place];
        for (int i = 1; i <= PropList.Count; i++)
        {
            int num2 = num + i;
            if (num2 > PropList.Count)
            {
                num2 -= (PropList.Count + 1);
            }
            if (num2 == 0 || (Attribute.Instance.filedInfo.SaveProps[0] != (Prop)num2 && Attribute.Instance.filedInfo.SaveProps[1] != (Prop)num2 && Attribute.Instance.filedInfo.SaveProps[2] != (Prop)num2))
            {
                Attribute.Instance.filedInfo.SaveProps[Place] = (Prop)num2;
                break;
            }
        }
        switch (Place)
        {
            case 0:
                text1.sprite = propuis[(int)Attribute.Instance.filedInfo.SaveProps[0]];
                break;
            case 1:
                text2.sprite = propuis[(int)Attribute.Instance.filedInfo.SaveProps[1]];
                break;
            case 2:
                text3.sprite = propuis[(int)Attribute.Instance.filedInfo.SaveProps[2]];
                break;
        }
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
    }
}
