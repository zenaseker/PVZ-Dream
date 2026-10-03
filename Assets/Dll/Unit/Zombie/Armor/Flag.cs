using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : Armor
{
    public override void ChangeSprite(float hp)
    {
        if (hp <= 0.5f)
        {
            aromorspr.sprite = tex1;
        }
    }

}
