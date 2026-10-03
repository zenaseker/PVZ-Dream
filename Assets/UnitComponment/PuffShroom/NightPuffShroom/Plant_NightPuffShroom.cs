using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_NightPuffShroom : Plant_PuffShroom
{
    public GameObject DefaultBody;
    public GameObject ShadowBody;
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        this.OnChangeMesh();
    }
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Dark", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Dark));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
    public override void OnChangeMesh()
    {
        List<PlantBase> flag2 = MapManage.Instance.meshPlants[this.XY.x, this.XY.y].GetPlants();
        flag2.Remove(this);
        bool flag = this.posType == PlantPosType.Little && flag2.Count > 0;
        DefaultBody.SetActive(!flag);
        ShadowBody.SetActive(flag);
        this.IgnoreUnit = flag;
    }
}
