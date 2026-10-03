using DG.Tweening;
using UnityEngine;

public class Plant_SunShroom : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        BuildTime = 120f;
        this.OnChangeMesh();
        DefaultProductTime = ProductTime = 24;
        Productingtime = 10;
    }
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        ProductTime = GetProductTime();
    }
    public override void Building()
    {
        this.transform.GetChild(0).DOScale(Vector3.one * 100, 1f);
        MusicManage.Instance.PlayEffect("plantgrow", 1f);
    }
    public override void OnChangeMesh()
    {
        base.OnChangeMesh();
        if (BuildTime <= 0)
        {
            if (this.posType == PlantPosType.Default)
            {
                this.transform.GetChild(0).localScale = new Vector3(100, 100, 1);
            }
            else if (this.posType == PlantPosType.Little)
            {
                this.transform.GetChild(0).localScale = new Vector3(65, 65, 1);
            }
        }
        else
        {
            if (this.posType == PlantPosType.Default)
            {
                isBuild = false;
            }
            else if (this.posType == PlantPosType.Little)
            {
                isBuild = true;
            }
        }
    }
    public override void Product()
    {
        base.Product();
        Invoke("InitSun", 1f);
        ChangeLight(1.3f,0.05f);
        HighLighttime = 0.1f;
    }
    private void InitSun()
    {
        OnProduct(BattleManage.CreateSun(ObjCreateTsf.position, (this.isBuild && this.posType != PlantPosType.Little) ? 25:15, true));
    }
}
