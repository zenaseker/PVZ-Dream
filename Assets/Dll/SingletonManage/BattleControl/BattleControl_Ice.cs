using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class BattleControl_Ice : BattleControlBase
{
    public override void OnLevelEnd()
    {
        if (!Attribute.Instance.filedInfo.tutorialKeys.Contains(TutorialKey.SecondDreamElementAdd))
        {
            Attribute.Instance.waitingtutorial = TutorialKey.SecondDreamElementAdd;
        }
    }
}
