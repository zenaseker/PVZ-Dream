using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Zombie_RedStar : ZombiesBase
{
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1f)
    {
        this.unitInfo = zombieCard.Clone();
        this.bufDetail = new UnitBufDetail(this);
        if (BattleManage.Instance.LevelDreamDepth > 0)
        {
            foreach (BattleUnitBuf zombieBufBase in BuffManage.Instance.GetZombieDreamDepthIncrease())
            {
                this.bufDetail.AddBuf(zombieBufBase);
            }
        }
        HP = MaxHP;
        Line = line;
        this.ChangeSpeed();
        this.GetComponent<SortingGroup>().sortingOrder = order;
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.gameObject.name != "shadow")
            {
                spriteRenderers.Add(spriteRenderer);
            }
        }
    }
    protected override void OnUpdate()
    {
        bufDetail.OnUpdate?.Invoke(Time.deltaTime);
    }
    public override void TakeDamage(DamageObject damageObject)
    {
        bufDetail.OnTakeDamage?.Invoke(damageObject.Damage, damageObject.DamageElement);
        if (this.HP <= 0) return;
        this.HP -= damageObject.Damage;
        if (damageObject.IsBroom || this.HP <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        bufDetail.OnDie?.Invoke();
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.Destroy();
    }
}
