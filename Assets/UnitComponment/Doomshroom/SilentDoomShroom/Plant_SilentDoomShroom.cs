using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_SilentDoomShroom : Plant_DroomShroom
{
    public override void Broom()
    {
        base.Broom();
        for (int i = 0; i < GameObject.Find("PlantManage").transform.childCount; i++)
        {
            if (GameObject.Find("PlantManage").transform.GetChild(i).TryGetComponent<BattleUnitModel>(out var model))
            {
                model.bufDetail.AddBuf(new BattleUnitBuf_SilentDoom(), 5);
                model.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 5);
            }
        }
        for (int i = 0; i < GameObject.Find("ZombieManage").transform.childCount; i++)
        {
            if (GameObject.Find("ZombieManage").transform.GetChild(i).TryGetComponent<BattleUnitModel>(out var model))
            {
                model.bufDetail.AddBuf(new BattleUnitBuf_SilentDoom(), 5);
                model.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 5);
            }
        }
    }
    public class BattleUnitBuf_SilentDoom : BattleUnitBuf
    {
        public override float TakeDamageChange(int dmg, DamageElement damageType)
        {
            if (this._owner.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion) != null)
            {
                this._owner.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, -1);
                if (this._owner is PlantBase) return 0.5f;
                if (this._owner is ZombiesBase) return 1.5f;
            }
            return base.TakeDamageChange(dmg, damageType);
        }
    }
}

