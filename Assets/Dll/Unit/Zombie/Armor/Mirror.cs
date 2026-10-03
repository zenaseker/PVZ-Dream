using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

public class Mirror : Armor
{
    public override void TakeDamage(DamageObject damageObject)
    {
        if (damageObject.DamageElement != DamageElement.Light)
        {
            this.Durable -= 20;
            RaycastHit2D[] hit2Ds = BattleManager.GetRayAll(this.transform.position, Vector2.left, 50f, 1 << LayerMask.NameToLayer("Unit"));
            if (hit2Ds != null && hit2Ds.Length > 0)
            {
                GameObject lightline = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "LightLine");
                lightline.GetComponent<LineRenderer>().SetPosition(0, this.ZombiesBase.transform.position + new Vector3(-1f,1f,0));
                lightline.GetComponent<LineRenderer>().SetPosition(1, MapManage.Instance.meshpos[this.ZombiesBase.Line, 0] + new Vector3(-1.5f, 1f, 0));
                RandomUtil.AddOrGetComponent<TimeDestory>(lightline).Init(0.2f);
                foreach(RaycastHit2D ray in hit2Ds)
                {
                    if (ray.transform.tag == "Plant")
                    {
                        ray.transform.GetComponent<PlantBase>().TakeDamage(new DamageObject(20, Bullettype.Zombie, this.ZombiesBase) { DamageElement = DamageElement.Light });
                    }
                }
            }
        }
        else
        {
            damageObject.Damage *= 2;
        }
        base.TakeDamage(damageObject);
    }
}
