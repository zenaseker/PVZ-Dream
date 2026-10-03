using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedOnceUse : Seed
{
    public override void Init(Attribute.PlantInfo plantCard)
    {
        this.card = plantCard;
        if (card == null)
        {
            Debug.Log("Error: PlantCard not found.");
            GameObject.Destroy(this.gameObject);
            return;
        }
        this.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "";
    }
    public override void Update()
    {
        return;
    }
    public override void OnCilck()
    {
        if (HandManage.Instance?.OriginCardSeed != null)
        {
            HandManage.Instance?.ClearHandPlant();
        }
        MusicManage.Instance.PlayEffect("seedlift", 1);
        if (PropManage.Instance?._propBase != null)
        {
            PropManage.Instance?.CancelProp();
        }
        HandManage.Instance?.AddPlant(this);
    }
    public override void ToCool(float time = 0)
    {
        Destory();
    }
    public override void Destory()
    {
        GameObject.Destroy(this.gameObject);
    }
    public override void OnDestroy()
    {
        return;
    }

}

