using DG.Tweening;
using UnityEngine;

public class Plant_SunFlower : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        DefaultProductTime = ProductTime = 24;
        Productingtime = 10;
    }
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        ProductTime = GetProductTime();
    }

    public override void Product()
    {
        base.Product();
        Invoke("InitSun", 1f);
        ChangeLight(1.4f,0.5f);
        HighLighttime = 1f;
    }
    private void InitSun()
    {
        OnProduct(BattleManage.CreateSun(ObjCreateTsf.position, 25, true));
    }
}
