using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultMapBuff;

public class Plant_DroomShroom : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        if (InSleep) return;
        this.GetComponent<Animator>().Play("Broom");
    }
    public override void OnTakeDamage(int dmg, BattleUnitModel attacker, DamageElement damageType = DamageElement.Default)
    {
        return;
    }
    public virtual void Broom()
    {
        Invoke("Die", 0.1f);
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Doom", this.transform.position);
        RandomUtil.AddOrGetComponent<Doom>(obj).Init(1800, true, 6.3f, 3f);
        InitBullet(gameObject.GetComponent<IEnchantment>());
        MapManage.Instance.meshPlants[this.XY.x, this.XY.y].AddMapBuf(new MapMeshUnitBuf_DoomCrater(MapManage.Instance.meshPlants[this.XY.x, this.XY.y],PlantPosType.All),1);
    }
}
