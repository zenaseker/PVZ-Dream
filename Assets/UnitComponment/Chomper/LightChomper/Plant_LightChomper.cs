using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_LightChomper : Plant_Chomper
{
    public int suncool = 0;
    public void CheckSunCool(int num)
    {
        suncool = num;
        this.transform.GetChild(0).Find("Chomper_spike1").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (num >= 5) ? 1f : 0.5f);
        this.transform.GetChild(0).Find("Chomper_spike2").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (num >= 10) ? 1f : 0.5f);
        this.transform.GetChild(0).Find("Chomper_spike3").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (num >= 15) ? 1f : 0.5f);
        this.transform.GetChild(0).Find("Chomper_spike4").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (num >= 20) ? 1f : 0.5f);
    }
    
    public override void AfterSwallow()
    {
        base.AfterSwallow();
        BattleManage.CreateSun(this.transform.position, 25, true);
    }
}
