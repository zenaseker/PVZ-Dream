using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_WuLong : ZombiesBase
{
    float time = 0f;
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1f)
    {
        base.Init(order, zombieCard, line, startspeed);
        bufDetail.GetBufList().Clear();
        ChangeSpeed();
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        time += Time.deltaTime;
        if (time > 30f && !this.Reverse)
        {
            GoBack();
        }
    }
    public override bool CanAddBuf(BattleUnitBuf buf)
    {
        return false;
    }
    public void GoBack()
    {
        this.transform.rotation = Quaternion.Euler(0f,180f,0f);
        this.Reverse = true;
    }
    public override void TakeDamage(DamageObject damageObject)
    {
        try
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.material.DOFloat(1.3f, "_HighLight", 0.05f).onComplete += () =>
                    {
                        if (spriteRenderer != null)
                        {
                            spriteRenderer?.material.DOFloat(1f, "_HighLight", 0.05f);
                        }
                    };
                }
            }
        }
        catch
        {

        }
        return;
    }
}
