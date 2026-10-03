using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Cardtype
{
    Ready,
    CanUse,
    InHand,
    WaitSun,
    Cool
}
public class Seed : MonoBehaviour
{
    public Attribute.PlantInfo card;
    //冷却倒计时
    public float cooltime = 0f;
    //处于冷却倒计时
    public Cardtype incool = Cardtype.Ready;

    public bool OnUse = false;

    public virtual void Init(Attribute.PlantInfo plantCard)
    {
        this.card = plantCard.Clone();
        if (card == null)
        {
            Debug.Log("Error: PlantCard not found.");
            GameObject.Destroy(this.gameObject);
            return;
        }
        this.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = this.card.Cost.ToString();
    }
    public virtual void OnCilck()
    {
        if (HandManage.Instance.OriginCardSeed != null)
        {
            HandManage.Instance.ClearHandPlant();
        }
        if (incool == Cardtype.CanUse || OnUse)
        {
            MusicManage.Instance.PlayEffect("seedlift", 1);
            if (PropManage.Instance._propBase != null)
            {
                PropManage.Instance.CancelProp();
            }
            HandManage.Instance.AddPlant(this);
        } 
    }

    public virtual void Update()
    {
        if (incool == Cardtype.Cool)
        {
            cooltime += Time.deltaTime;
            float num = cooltime / card.UseCoolTime;
            this.transform.GetChild(2).GetComponent<Image>().fillAmount = 1 - num;
            if (num >= 1)
            {
                cooltime = 0f;
                incool = Cardtype.CanUse;
                this.transform.GetChild(4).gameObject.SetActive(false);
                SunEnoughorNot();
            }
        }
    }

    public virtual void OnDestroy()
    {
        BattleManage.Instance.checkCardCost -= this.SunEnoughorNot;
    }

    public virtual void ToCool(float time = 0f)
    {
        if (OnUse)
        {
            this.Destory();
            return;
        }
        cooltime = time;
        this.transform.GetChild(2).gameObject.SetActive(true);
        this.transform.GetChild(4).gameObject.SetActive(true);
        incool = Cardtype.Cool;
    }
    public void ClearCool()
    {
        cooltime = 0;
        this.transform.GetChild(2).GetComponent<Image>().fillAmount = 0;
        this.transform.GetChild(4).gameObject.SetActive(false);
        incool = Cardtype.WaitSun;
        SunEnoughorNot();
    }
    public virtual void Destory()
    {
        this.OnDestroy();
        GameObject.Destroy(this.gameObject);
    }
    public void SunEnoughorNot()
    {
        if (BattleManage.Instance.SunNumber < this.card.Cost)
        {
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            if (incool == Cardtype.CanUse)
            {
                incool = Cardtype.WaitSun;
            }
        }
        else
        {
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            if (incool == Cardtype.WaitSun)
            {
                incool = Cardtype.CanUse;
            }
        }
    }
}
