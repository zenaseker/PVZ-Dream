using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class BattleControl_Light : BattleControlBase
{
    public override void OnLevelStart()
    {
        DebugShow.Instance.Init("梦境深度——随着你种植的梦境植物种类越多，梦境深度随之增加。", 15f);
    }
    public override void OnChangeElement(Attribute.DreamElement dreamElement)
    {
        DebugShow.Instance.Init("梦境深度会增强僵尸的各个能力，如生命上限、速度、伤害减免等等。", 15f);
    }
}
