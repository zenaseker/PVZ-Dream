
using UnityEngine;
using UnityEngine.UI;

public class BattleControl_Chainreaction : BattleControlBase
{
    public override void OnLevelStart()
    {
        base.OnLevelStart();
        DebugShow.Instance.Init("本关GN猫猫的攻击力和价格减半！");
        if (UImanage.Instance != null)
        {
            if (UImanage.Instance.BankList.GetChild(0).TryGetComponent<Seed>(out Seed component))
            {
                component.card.Cost = 115;
                component.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "115";
            }
        }
    }
    public override void OnCellPlant(PlantBase plant)
    {
        base.OnCellPlant(plant);
        plant.unitInfo.Damage /= 2;
    }
}