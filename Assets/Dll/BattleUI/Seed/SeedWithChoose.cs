using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//战前选卡
public class SeedWithChoose : MonoBehaviour
{
    public enum CardIn
    {
        SeedChooser,
        SeedBankbyReady,
        SeedBankbyBattle
    }
    public Seed seed = null;
    public ElementSeed elementSeed = null;
    //卡位置
    public CardIn cardIn = CardIn.SeedChooser;
    //选卡界面原卡
    public SeedWithChoose SeedChooseOrigin = null;
    //是否已选卡
    public bool Choosed = false;

    public void Init(Seed seed = null,ElementSeed elementSeed = null)
    {
        this.seed = seed;
        this.elementSeed = elementSeed;
        this.GetComponent<Button>().onClick.AddListener(this.OnClick);
    }
    public void OnClick()
    {
        switch (cardIn)
        {
            case CardIn.SeedChooser:
                this.ChooseCard();
                break;
            case CardIn.SeedBankbyReady:
                this.ReChooseCard();
                break;
            case CardIn.SeedBankbyBattle:
                break;
        }
    }

    public void ChooseCard()
    {
        if (Choosed) return;
        if (seed != null)
        {
            UImanage.Instance.InitCard(this);
        }
        else
        {
            UImanage.Instance.InitElementCard(this);
        }
    }
    public void ChooseCardInit(bool flag)
    {
        this.Choosed = flag;
        if (seed != null)
        {
            this.transform.GetChild(3).gameObject.SetActive(flag);
        }
        else
        {
            this.transform.GetChild(2).gameObject.SetActive(flag);
        }
    }
    public void ReChooseCard()
    {
        if (seed != null)
        {
            UImanage.Instance.ReInitCard(SeedChooseOrigin, this);
        }
        else
        {
            UImanage.Instance.ReInitElementCard(SeedChooseOrigin,this);
        }
    }
}
