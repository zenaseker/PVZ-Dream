using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Potato : Zombie_Common
{
    float time = 0f;
    bool isbuild = false;
    public GameObject red;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!isbuild)
        {
            time += Time.deltaTime;
            if (time > 25f)
            {
                isbuild = true;
                red.SetActive(true);
            }
        }
    }
    public override void OnPlantContact(PlantBase plant)
    {
        base.OnPlantContact(plant);
        if (isbuild)
        {
            MusicManage.Instance.PlayEffect("potato_mine", 1f);
            foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Unit")))
            {
                if (monster.gameObject.tag == "Plant")
                {
                    monster.GetComponent<PlantBase>().TakeDamage(new DamageObject(500, Bullettype.PotatoMine, this));
                }
            }
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "PotatoMineBroom", this.transform.position+Vector3.up);
            RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
            this.Die();
        }
    }
}
