using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_DawnDoublePea : Plant_DoublePea
{
    public override void OnAttack()
    {
        if (this.component._matrix._RegetMatrixnum >= 6)
        {
            if (this.component._matrix._RegetMatrixnum >= 12)
            {
                if (this.XY.x > 0)
                {
                    GameObject gameObject3 = PoolManage.Instance.GetPoolGameObject("Bullet", "LightPeaBullet", ObjCreateTsf.position);
                    gameObject3.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage / 2, DamageElement.Light));
                    gameObject3.transform.localScale = Vector3.one;
                    gameObject3.GetComponent<BulletBase>().ChangeLine(this.XY.x - 1);
                    InitBullet(gameObject3.GetComponent<IEnchantment>());
                }
                if (this.XY.x < MapManage.Instance.meshxy.x - 1)
                {
                    GameObject gameObject3 = PoolManage.Instance.GetPoolGameObject("Bullet", "LightPeaBullet", ObjCreateTsf.position);
                    gameObject3.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage / 2, DamageElement.Light));
                    gameObject3.transform.localScale = Vector3.one;
                    gameObject3.GetComponent<BulletBase>().ChangeLine(this.XY.x + 1);
                    InitBullet(gameObject3.GetComponent<IEnchantment>());
                }
            }
            GameObject gameObject2 = PoolManage.Instance.GetPoolGameObject("Bullet", "LightPeaBullet2", ObjCreateTsf.position);
            gameObject2.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage * 2, DamageElement.Light));
            InitBullet(gameObject2.GetComponent<IEnchantment>());
            return;
        }
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "LightPeaBullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Light));
        gameObject.transform.localScale = Vector3.one * 2;
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
