using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLose : MonoBehaviour
{
    public void AfterAnimator()
    {
        GameDebugUI.Instance.Init("游戏结束", "重新开始", null, "返回主菜单", Left, null, Right);
    }
    public void Right()
    {
        BattleManage.Instance.ReturnMianMenu(false);
    }
    public void Left()
    {
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        BattleManage.Instance.ReStart();
    }
}
