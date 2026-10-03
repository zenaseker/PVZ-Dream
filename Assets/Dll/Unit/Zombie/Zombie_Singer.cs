using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Singer : Zombie_Common
{
    float time = 0f;
    int count = 0;
    GameObject effect;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        time += Time.deltaTime;
        if (time > 10f)
        {
            count = 0;
            this.GetComponent<Animator>().SetBool("Sing", true);
            effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "SIngerArea", this.transform);
            effect.transform.localPosition = Vector3.zero;
            time = 0;
        }
    }
    public void Sing()
    {
        count++;
        if (count >= 5)
        {
            count = 0;
            this.GetComponent<Animator>().SetBool("Sing", false);
            PoolManage.Instance.PushGameObject(effect.name, effect, true);
            time = 0;
        }
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 2.5f, 2))
        {
            if (monster.gameObject.tag == "Zombie")
            {
                ZombiesBase zombie = monster.gameObject.GetComponent<ZombiesBase>();
                if (zombie.HP * 3 > zombie.MaxHP * 2)
                {
                    zombie.bufDetail.AddBuf(new BuffManage.BattleUnitBuf_AttackUp(), 1, 10f);
                }
                else if (zombie.HP * 3 > zombie.MaxHP)
                {
                    zombie.bufDetail.AddBuf(new BuffManage.BattleUnitBuf_DamageDown(), 1, 10f);
                }
                else
                {
                    zombie.ReCoverHp(100);
                }
            }
        }
    }
    public override void LoseArm()
    {
        this.losearm = true;
    }
}
