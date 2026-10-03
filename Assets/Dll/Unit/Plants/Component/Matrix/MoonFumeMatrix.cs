

public class MoonFumeMatrix : MatrixBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = MapManage.Instance.GetEffectiveRange(detail._owner.XY, 1.5f);
        base.Init(detail);
    }
    public override bool MatrixGetJudge(PlantBase plant)
    {
        return plant != null && plant.unitInfo.DreamElement.Contains(Attribute.DreamElement.Dark);
    }
}