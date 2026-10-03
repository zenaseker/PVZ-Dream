using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Chomper : Zombie_Common
{
    float time = 0f;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        time -= Time.deltaTime;
    }
    public override void Attack()
    {
        base.Attack();
        if (time <= 0f)
        {
            time = 40f;
            if (unitbase is PlantBase) (unitbase as PlantBase).Die();
            if (unitbase is ZombiesBase) (unitbase as ZombiesBase).Die();
        }
    }
    public override void LoseHead()
    {
        this.losehead = true;
        if (isdie) return;
        Invoke("DieAfterLoseHead", 2f);
    }
}
