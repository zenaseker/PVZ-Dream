using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : Armor
{
    public override void Destory()
    {
        base.Destory();
        MusicManage.Instance.PlayEffect("newspaper_rip",1);
    }
}
