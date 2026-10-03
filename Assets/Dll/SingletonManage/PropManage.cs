using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropManage : Singleton<PropManage>
{
    public Dictionary<Prop, PropBase> PropList = new Dictionary<Prop, PropBase>();
    public Itemcooldown _Button = null;
    public PropBase _propBase = null;
    public Transform PropUIButtonList;
    public Transform PropsList;
    public Transform FastKeyInput;
    public static bool IgnoreCoolTime = false;
    bool show = true;
    public void Start()
    {
        BattleManage.Instance._BackSellChoice += CreateProps;
        this.transform.Find("PropChoice").gameObject.SetActive(true);
        this.transform.Find("PropUIButtonList").gameObject.SetActive(false);
    }
    public void CreateProps()
    {
        this.GetComponent<Animator>().SetBool("INBattle", true);
        if (show) { this.GetComponent<Animator>().Play("Hide2"); }
        this.transform.Find("PropChoice").gameObject.SetActive(false);
        this.transform.Find("PropUIButtonList").gameObject.SetActive(true);
        if (Attribute.Instance.filedInfo.SaveProps == null) return;
        PropList = new Dictionary<Prop, PropBase>();
        _Button = null;
        _propBase = null;
        for (int i = 0; i < 3; i++)
        {
            Prop prop = Attribute.Instance.filedInfo.SaveProps[i];
            if (prop != 0 && !PropList.ContainsKey(prop))
            {
                GameObject propbutton = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/PropUI/" + prop.ToString() + "Bank"), PropUIButtonList);
                GameObject propobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Prop/" + prop.ToString()), PropsList);
                PropList.Add(prop, propobj.GetComponent<PropBase>());
                propobj.SetActive(false);
                FastKeyInput.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            this.transform.Find("PropUIButtonList").GetChild(0)?.GetComponent<Itemcooldown>()?.OnClick();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            this.transform.Find("PropUIButtonList").GetChild(1)?.GetComponent<Itemcooldown>()?.OnClick();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            this.transform.Find("PropUIButtonList").GetChild(2)?.GetComponent<Itemcooldown>()?.OnClick();
        }
    }
    public void OnButtonClick(Itemcooldown button, Prop prop)
    {
        CancelProp();
        _Button = button;
        _Button.ShoworHide(false);
        _propBase = PropList[prop];
        _propBase.ShowProp();
        _propBase.transform.position = button.transform.position;
    }
    public void CancelProp()
    {
        if (_Button != null)
        {
            _Button.ShoworHide(true);
            _Button = null;
        }
        if (_propBase != null)
        {
            _propBase.HideProp();
            _propBase = null;
        }
    }
    public void OnPropUsed()
    {
        if (_Button != null)
        {
            _Button.Gocooltime();
            _Button = null;
        }
        if (_propBase != null)
        {
            _propBase.HideProp();
            _propBase = null;
        }
    }
    public void ButtonShowOrHide()
    {
        if (BattleManage.Instance.battleStage == BattleStage.ChooseCard) return;
        if (!show)
        {
            this.GetComponent<Animator>().Play("Show");
        }
        else
        {
            this.GetComponent<Animator>().Play("Hide");
        }
        show = !show;
    }
    private void OnDestroy()
    {
        BattleManage.Instance._BackSellChoice -= CreateProps;
    }
}
