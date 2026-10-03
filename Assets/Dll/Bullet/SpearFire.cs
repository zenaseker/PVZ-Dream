using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearFire : BulletBase
{
    public override void Init(Vector2 flyspeed, int startline, UnitFaction unitFaction, int dmg = -1)
    {
        base.Init(flyspeed, startline, unitFaction, dmg);
        this.transform.DORotate(new Vector3(0,0,35f), 0.35f);
        this.transform.DOPath(new Vector3[] { this.transform.position - Vector3.up * 0.1f, new Vector3(this.transform.position.x - 0.1f, this.transform.position.y), new Vector3(this.transform.position.x - 2.6f, this.transform.position.y - 1.5f) }, 0.4f,PathType.CatmullRom)
            .onComplete += Full;
    }
    public override void OnHit(BattleUnitModel unit)
    {
        base.OnHit(unit);
        if (((Attribute.PlantInfo)unit.unitInfo).Planttype == PlantType.Defensive)
        {
            PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
        }
    }
    public void Full()
    {
        PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
    }
}
