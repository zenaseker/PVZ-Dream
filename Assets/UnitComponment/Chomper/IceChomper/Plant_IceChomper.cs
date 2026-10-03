using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plant_IceChomper : Plant_Chomper
{
    int EatZombienum = 0;
    public override void OnSucessEat(ZombiesBase zombie)
    {
        base.OnSucessEat(zombie);
        if (EatZombienum < 4)
        {
            EatZombienum++;
        }
        foreach(string text in Attribute.Instance.ComponentList.Keys)
        {
            if (text.Contains("Ice") && text.Contains("Enchantment"))
            {
                if (Attribute.Instance.ComponentList[text] is EnchantmentBase && !this.component._enchantment.Contains(Attribute.Instance.ComponentList[text]))
                {
                    this.component._enchantment.Add((EnchantmentBase)Attribute.Instance.ComponentList[text]);
                }
            }
        }
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStreamSector",this.transform.position);
        RandomUtil.AddOrGetComponent<FrostStreamSector>(obj).Init(EatZombienum,3f, 1);
    }
    public override void AfterSwallow()
    {
        base.AfterSwallow();
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStreamSector", this.transform.position);
        RandomUtil.AddOrGetComponent<FrostStreamSector>(obj).Init(EatZombienum,3f, 1);
    }
}
