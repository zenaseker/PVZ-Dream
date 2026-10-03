using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishtowl : Armor
{
    public override float DamageReDuction(int dmg, DamageElement type)
    {
        if (type != DamageElement.Default)
        {
            return 0.5f;
        }
        return base.DamageReDuction(dmg, type);
    }
}
