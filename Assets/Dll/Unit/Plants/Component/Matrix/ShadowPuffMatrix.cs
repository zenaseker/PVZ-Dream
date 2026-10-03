using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class ShadowPuffMatrix : MatrixBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = new List<Vector2Int>{detail._owner.XY};
        base.Init(detail);
    }
    public override bool MatrixGetJudge(PlantBase plant)
    {
        return plant != null && plant != this._detail._owner;
    }
}