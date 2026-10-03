using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Armor
{
    public override void TakeDamage(DamageObject damageObject)
    {
        if (damageObject.Bullettype == Bullettype.PuffShroom)
        {
            damageObject.IgnoreAromor2 = true;
            this.ZombiesBase.TakeDamage(damageObject);
        }
        base.TakeDamage(damageObject);
    }
}
