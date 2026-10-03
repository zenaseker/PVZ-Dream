using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl_Ice2 : BattleControlBase
{
    public override void BeforeSelcetCard()
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.SecondDreamElementAdd)
        {
            DebugShow.Instance.Init("你获得了第二个元素——冰元素，让我们将其添加至上方列表", 99f);
        }
    }
    public override void OnLevelStart()
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.SecondDreamElementAdd)
        {
            DebugShow.Instance.Init("在战斗中，你可以同时激活多个元素，植物卡片将优先染色为你最后激活的元素", 99f);
        }
    }
    public override void OnChangeElement(Attribute.DreamElement dreamElement)
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.SecondDreamElementAdd)
        {
            DebugShow.Instance.Init("灵活的切换元素应对更多的僵尸吧！", 5f);
            Attribute.Instance.waitingtutorial = TutorialKey.None;
            Attribute.SaveTutorial(TutorialKey.SecondDreamElementAdd);
        }
    }
}
