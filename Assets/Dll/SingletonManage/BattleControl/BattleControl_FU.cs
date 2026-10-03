
using UnityEngine;

public class BattleControl_FU : BattleControlBase
{
    public override int ChangeCreateZombie(int OrginId)
    {
        if (Random.Range(0, 100) > 30)
        {
            return 105;
        }
        return base.ChangeCreateZombie(OrginId);
    }
}