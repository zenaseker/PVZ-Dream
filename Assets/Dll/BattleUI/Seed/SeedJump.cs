using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedJump : Seed
{
    public float time = 60f;
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
        this.transform.GetChild(0).gameObject.SetActive(true);
        RandomUtil.AddOrGetComponent<ObjectJump>(this.gameObject).Move(Vector3.zero);
    }
    public override void Update()
    {
        time -= Time.deltaTime;
        this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Sin(time));
        if (time <= 10f)
        {
            this.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Sin(time * 3));
        }
        if (time <= 0f)
        {
            Destory();
        }
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
