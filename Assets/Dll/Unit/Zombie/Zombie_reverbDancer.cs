using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_reverbDancer : Zombie_Dancer
{
    public Sprite[] head = new Sprite[4];
    public SpriteRenderer _head;
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1)
    {
        base.Init(order, zombieCard, line, startspeed);
        _head.sprite = head[Random.Range(0,4)];
    }
    public override void LoseArm()
    {
        return;
    }
    public override void LoseHead()
    {
        this.losehead = true;
        if (isdie) return;
        Invoke("DieAfterLoseHead", 2f);
    }
    public override void Attack()
    {
        if (unitbase == null)
        {
            this.GetComponent<Animator>().SetBool("Attack", false);
            return;
        }
        MusicManage.Instance.PlayEffect("reverbDancer", 1f);
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 1f, 1 << LayerMask.NameToLayer("Unit")))
        {
            if (monster.gameObject.tag == "Plant")
            {
                DamageObject damageObject = new DamageObject(100, Bullettype.Zombie, this)
                {
                    DamageElement = DamageElement.Soul,
                };
                monster.GetComponent<PlantBase>().TakeDamage(damageObject);
                bufDetail.OnAttack?.Invoke(damageObject);
            }
        }
    }
}
