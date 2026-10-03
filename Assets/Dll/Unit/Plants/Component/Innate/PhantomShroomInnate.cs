using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomShroomInnate : InnateBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Init(detail);
        this._detail._owner.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion,1).Maxstack = 50;

    }
}
