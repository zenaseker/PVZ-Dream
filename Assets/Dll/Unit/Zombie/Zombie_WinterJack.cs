using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_WinterJack : Zombie_Common
{
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1)
    {
        base.Init(order, zombieCard, line, startspeed);
        this.bufDetail.AddBuf(new BattleUnitBuf_WinterJack());
    }
    public override bool CanAddBuf(BattleUnitBuf buf)
    {
        return base.CanAddBuf(buf) && buf.KeyWordBuf != KeyWordBuf.Cold;
    }
    public override void LoseArm()
    {
        this.losearm = true;
        transform.GetChild(0).Find("×ó¼ç").GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).Find("×ó¼ç").GetChild(1).gameObject.SetActive(false);
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieWinterJackArm", transform.GetChild(0).Find("×ó¼ç").GetChild(0).position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.6f, 0.6f);
    }
    public class BattleUnitBuf_WinterJack : BattleUnitBuf
    {
        public override float TakeDamageChange(int dmg,DamageElement damageType)
        {
            if (damageType == DamageElement.Snow)
            {
                return -0.5f;
            }
            return base.TakeDamageChange(dmg,damageType);
        }
    }

}
