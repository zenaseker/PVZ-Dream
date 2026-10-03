using static Attribute;

public class HalationMatrix : MatrixBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = MapManage.Instance.GetEffectiveLine(detail._owner.XY, RayDirection.left);
        base.Init(detail);
    }
    public override bool MatrixGetJudge(PlantBase plant)
    {
        return plant != null && ((Attribute.PlantInfo)plant.unitInfo).Planttype == PlantType.Production;
    }
    public override bool MatrixGetJudge2(PlantBase plant)
    {
        return plant != null && ((Attribute.PlantInfo)plant.unitInfo).Planttype == PlantType.Attack;
    }
}