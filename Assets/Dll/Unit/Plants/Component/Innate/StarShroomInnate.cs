
public class StarShroomInnate : InnateBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Init(detail);
        for(int i = 0; i < HandManage.Instance.PlantManage.transform.childCount; i++)
        {
            if (HandManage.Instance.PlantManage.transform.GetChild(i).gameObject.tag == "Plant")
            {
               if (HandManage.Instance.PlantManage.transform.GetChild(i).GetComponent<PlantBase>() != null && HandManage.Instance.PlantManage.transform.GetChild(i).GetComponent<PlantBase>() is Plant_StarShroom)
               {
                   this._detail._owner.OnProduct(BattleManage.CreateSun(HandManage.Instance.PlantManage.transform.GetChild(i).position, 10, true));
               }
            }
        }
    }
}
