using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plant_PeaByOTT : PlantBase
{
    public float coolltime = 0f;
    public bool iszhuantou = false;

    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        coolltime -= Time.deltaTime;
        if (coolltime <= 0)
        {
            if (iszhuantou)
            {
                Zhuantou(false);
                this.ChangeLight(1);
                coolltime = 5;
            }
            else
            {
                this.ChangeLight(1.3f, 10);
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Zhuantou(true);
                    BattleControl_OneTwoThree.Stop();
                    this.ChangeLight(1);
                    coolltime = 1f;
                }
            }
        }
    }

    void Zhuantou(bool flag)
    {
        this.transform.rotation = Quaternion.Euler(0, flag?0:180, 0);
        iszhuantou = flag;
    }
}
