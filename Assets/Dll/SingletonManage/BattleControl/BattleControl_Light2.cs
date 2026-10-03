using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class BattleControl_Light2 : BattleControlBase
{
    public override void BeforeSelcetCard()
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.FirstDreamElementAdd)
        {
            DebugShow.Instance.Init("恭喜你获得了第一个元素——光元素，点击右侧元素列表中的光元素将其加入上方列表", 99f);
        }
    }
    public override void OnSelcetSeedCard(ElementSeed seed)
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.FirstDreamElementAdd)
        {
            if (seed.dreamElement == Attribute.DreamElement.Light)
            {
                DebugShow.Instance.Init("光元素是生活中无处不在的元素，现在选择植物开始战斗吧！", 99f);
            }
        }
    }
    public override void OnLevelStart()
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.FirstDreamElementAdd)
        {
            DebugShow.Instance.Init("光元素是可以将你的植物卡片染色为对应的光元素梦境植物，点击光元素", 99f);
        }
    }
    public override void OnChangeElement(Attribute.DreamElement dreamElement)
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.FirstDreamElementAdd)
        {
            DebugShow.Instance.Init("元素在激活后会在60秒后熄灭，在此之前你可以随时熄灭它，\n熄灭后你需要等待一段时间才可重新将其激活，等待时间同等于你上次激活的时间且至少为15秒，\n现在选择并种植你的植物", 99f);
        }
    }
    public override void OnCellPlant(PlantBase plant)
    {
        if (Attribute.Instance.waitingtutorial == TutorialKey.FirstDreamElementAdd)
        {
            DebugShow.Instance.Init("看起来你已经完全掌握了！好好享受吧！");
            Attribute.Instance.waitingtutorial = TutorialKey.None;
            Attribute.SaveTutorial(TutorialKey.FirstDreamElementAdd);
        }
    }
}
