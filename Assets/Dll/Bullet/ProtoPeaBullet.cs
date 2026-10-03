using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoPeaBullet : PeaBullet
{
    public override void OnHit(BattleUnitModel unit)
    {
        base.OnHit(unit);
        if (unit is ZombiesBase) unit.transform.position += Vector3.right * 0.5f;
    }
}
