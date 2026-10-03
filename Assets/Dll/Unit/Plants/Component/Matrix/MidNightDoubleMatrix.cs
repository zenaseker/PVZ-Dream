
using System.Collections.Generic;
using UnityEngine;

public class MidNightDoubleMatrix : MatrixBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = new List<Vector2Int> { detail._owner.XY };
        base.Init(detail);
    }
    public override bool MatrixGetJudge(PlantBase plant)
    {
        return plant != null && plant.unitInfo.DreamElement.Contains(Attribute.DreamElement.Dark);
    }
}