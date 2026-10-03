using static Attribute;

public class LunarEclipseScaredyShroomMatrix : MatrixBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = MapManage.Instance.GetEffectiveRange(detail._owner.XY, 1.5f);
        base.Init(detail);
    }
    public override bool MatrixGetJudge(PlantBase plant)
    {
        return plant != null && ((Attribute.PlantInfo)plant.unitInfo).Planttype == PlantType.Production;
    }
}