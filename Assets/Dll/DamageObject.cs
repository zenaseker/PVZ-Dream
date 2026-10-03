using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject
{
    public int Damage = 0;
    public DamageElement DamageElement = DamageElement.Default;
    public bool IgnoreArmor = false;
    public bool IgnoreAromor2 = false;
    public bool IsBroom = false;
    public Bullettype Bullettype;
    public BattleUnitModel OriginUnit = null;
    public bool ToFly = false;//¶Ô¿Õ

    public DamageObject(int damage, Bullettype bullettype, BattleUnitModel originUnit)
    {
        Damage = damage;
        Bullettype = bullettype;
        OriginUnit = originUnit;
    }
}
