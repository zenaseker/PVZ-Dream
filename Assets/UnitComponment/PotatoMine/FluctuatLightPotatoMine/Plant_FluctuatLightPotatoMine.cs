using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_FluctuatLightPotatoMine : Plant_PotatoMine
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        BuildTime += 25f;
        Debug.Log("Bulidtime" + BuildTime);
    }
}
