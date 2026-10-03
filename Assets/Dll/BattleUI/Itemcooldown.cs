using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Prop
{
    None,
    Shovel,//铲子
    Hammer,//锤子
    Glove,//手套
    Fertilize,//肥料
    WateringCan,//水壶
}
public class Itemcooldown : MonoBehaviour
{
    public GameObject Item;
    public Image CoolItem;
    public Prop _PropItem;
    public float MaxCooltime = 30f;
    private float cooltime = 0f;
    bool incool = false;
    void Update()
    {
        if (incool)
        {
            cooltime -= Time.deltaTime;
            CoolItem.fillAmount = cooltime / MaxCooltime;
            if(cooltime < 0f)
            {
                incool = false;
                this.GetComponent<Button>().enabled = true;
            }
        }
    }
    public void Gocooltime()
    {
        cooltime = MaxCooltime;
        incool = true;
        ShoworHide(true);
        this.GetComponent<Button>().enabled = false;
        if (PropManage.IgnoreCoolTime)
        {
            cooltime = 0f;
        }
    }
    public void ShoworHide(bool flag)
    {
        Item.SetActive(flag);
    }
    public void OnClick()
    {
        if (BattleManage.Instance.battleStage == BattleStage.ChooseCard)
        {

        }
        if (PropManage.Instance._Button == this || incool)
        {
            PropManage.Instance.CancelProp();
            return;
        }
        PropManage.Instance.OnButtonClick(this, _PropItem);
        ShoworHide(false);
    }
}
