
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Attribute;

public class Plant_FireFlyPeaShooter : Plant_PeaShooter
{
    public SpriteRenderer _light;

    public override void Init(Vector2Int xy, PlantInfo plant)
    {
        base.Init(xy, plant);
        BattleManage.Instance.checkCardCost += LightChange;
    }
    public void LightChange()
    {
        float sun = (float)BattleManage.Instance.SunNumber / 500;
        if (sun >= 1f)
        {
            sun = 1f;
        }
        _light.color = new Color(1f, 1f,1f, sun);
    }
    public override void OnAttack()
    {
        if (Random.Range(0f,1f) <= 0.28f + component._matrix._RegetMatrixnum * 0.09f)
        {
            GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "LightPeaBullet", ObjCreateTsf.position);
            gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), (int)this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Light));
            gameObject.transform.localScale = Vector3.one * 2;
            InitBullet(gameObject.GetComponent<IEnchantment>());
            return;
        }
        base.OnAttack();
    }
    public override void Destory()
    {
        BattleManage.Instance.checkCardCost -= LightChange;
        base.Destory();
    }
}
