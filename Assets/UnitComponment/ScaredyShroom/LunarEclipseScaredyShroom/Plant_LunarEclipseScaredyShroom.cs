using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_LunarEclipseScaredyShroom : Plant_ScaredyShroom
{
    float nearzombiecooltime = 15f;
    float atrixcooltime = 0f;
    float atrixcooltimeback = 0f;

    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        if (this.component._matrix._RegetMatrixnum > 0)
        {
            atrixcooltime += Time.deltaTime;
            if (atrixcooltime >= 15)
            {
                atrixcooltime = 0;
                this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, -1);
            }
        }
        else
        {
            atrixcooltimeback += Time.deltaTime;
            if (atrixcooltimeback >= 30)
            {
                atrixcooltimeback = 0;
                this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
            }
        }
        //Debug.Log(this.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).Maxstack + " " + this.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).stack);
    }
    public override void overhaszombie()
    {
        base.overhaszombie();
        nearzombiecooltime -= Time.deltaTime;
        if (nearzombiecooltime <= 0)
        {
            nearzombiecooltime = 15;
            this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
        }
    }
    public override void OnAttack()
    {
        base.OnAttack();
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Dark", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f,0), this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Dark));
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
}
