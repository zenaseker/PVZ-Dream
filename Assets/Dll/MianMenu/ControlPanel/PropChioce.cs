using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropChioce : MonoBehaviour
{
    public GameObject buttonhelp1;
    public GameObject buttonhelp2;
    public GameObject buttonhelp3;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    List<Prop> PropList;
    static Dictionary<Prop,string> PropName = new Dictionary<Prop, string>()
    {
        {Prop.None,"无" },
        {Prop.Shovel,"铲子" },
        {Prop.Hammer,"锤子" },
        {Prop.Glove,"手套" },
        {Prop.Fertilize,"肥料" },
        {Prop.WateringCan,"水壶" },
    };

    public void OnEnable()
    {
        PropList = new List<Prop>();
        if (Attribute.Instance.filedInfo.MianFinishLevel >= 0)
        {
            PropList.Add(Prop.Shovel);
            buttonhelp1.SetActive(false);
        }
        else
        {
            text1.transform.parent.GetComponent<Button>().enabled = false;
        }

        if (Attribute.Instance.filedInfo.MianFinishLevel >= 8)
        {
            PropList.Add(Prop.Hammer);
        }

        if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(100))
        {
            PropList.Add(Prop.Glove);
            buttonhelp2.SetActive(false);
        }
        else
        {
            text2.transform.parent.GetComponent<Button>().enabled = false;
        }

        if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(200))
        {
            PropList.Add(Prop.WateringCan);
            buttonhelp3.SetActive(false);
        }
        else
        {
            text3.transform.parent.GetComponent<Button>().enabled = false;
        }

        if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(300))
        {
            PropList.Add(Prop.Fertilize);
        }

        if (Attribute.Instance.filedInfo.SaveProps != null)
        {
            text1.text = PropChioce.PropName[Attribute.Instance.filedInfo.SaveProps[0]];
            text2.text = PropChioce.PropName[Attribute.Instance.filedInfo.SaveProps[1]];
            text3.text = PropChioce.PropName[Attribute.Instance.filedInfo.SaveProps[2]];
        }
        else
        {
            text1.text = "无";
            text2.text = "无";
            text3.text = "无";
        }
    }

    public void SaveProp(int Place)
    {
        if (Attribute.Instance.filedInfo.SaveProps == null)
        {
            Attribute.Instance.filedInfo.SaveProps = new Prop[3] { 0, 0, 0 };
        }
        int num = (int)Attribute.Instance.filedInfo.SaveProps[Place];
        for(int i = 1; i <= PropList.Count; i++)
        {
            int num2 = num + i;
            if (num2 > PropList.Count)
            {
                num2 -= (PropList.Count +1);
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
                text1.text = PropChioce.PropName[Attribute.Instance.filedInfo.SaveProps[Place]];
                break;
            case 1:
                text2.text = PropChioce.PropName[Attribute.Instance.filedInfo.SaveProps[Place]];
                break;
            case 2:
                text3.text = PropChioce.PropName[Attribute.Instance.filedInfo.SaveProps[Place]];
                break;
        }
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
    }
}
