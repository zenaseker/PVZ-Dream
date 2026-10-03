using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Mirror : Zombie_Door
{

    public override void OnLoseArmor(Armor armor)
    {
        base.OnLoseArmor(armor);
        if (armor is Mirror)
        {
            this.Changearm(false);
        }
    }
}
