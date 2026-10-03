using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ParitcleSystemBase : MonoBehaviour, IEnchantment
{
    public Action<BattleUnitModel,int> Action { get; set; }

    public virtual void Init(float area,float effectarea,float time = 10f)
    {
        gameObject.AddComponent<TimeDestory>().Init(time);
        this.transform.localScale = Vector3.one * effectarea;
        for (int i = 0;i < this.transform.childCount;i++)
        {
            this.transform.GetChild(i).localScale = Vector3.one * effectarea;
        }
        Hits(area);
        Action = null;
    }
    public virtual void Hits(float area)
    {

    }
    public virtual void Hit(BattleUnitModel model,int dmg, DamageElement damageType, bool ignorearmor = false, bool isbroom = false)
    {
        Action?.Invoke(model, dmg);
    }
    public virtual void Hit(BattleUnitModel model)
    {
        Action?.Invoke(model, 0);
    }
}
