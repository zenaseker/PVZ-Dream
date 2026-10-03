using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zombie_EndingJackson : Zombie_Jackson
{
    List<Vector2Int> dancerpos = new List<Vector2Int> {new(0, 1), new(0, -1) };
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed)
    {
        base.Init(order, zombieCard, line, startspeed);
        this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 8).Maxstack = 72;
        if (this.Line != MapManage.Instance.meshxy.x - 1)
        {
            dancerpos.Add(new(1, 1));
            dancerpos.Add(new(1, 0));
            dancerpos.Add(new(1, -1));
        }
        if (this.Line != 0)
        {
            dancerpos.Add(new(-1, 1));
            dancerpos.Add(new(-1, 0));
            dancerpos.Add(new(-1, -1));
        }
    }

    protected override void AnimSummon()
    {
        int num = 2;
        if (this.Line != MapManage.Instance.meshxy.x - 1)
        {
            num++;
        }
        if (this.Line != 0)
        {
            num++;
        }
        if (this.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion)?.stack > 8)
        {
            while (this.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion)?.stack > 8 && dancerpos.Count > 0)
            {
                Vector2Int pos = RandomUtil.SelectOne(dancerpos);
                this.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).AddStack(-8, 0);
                GameObject obj = ZombieManage.Instance.InitZombie(8, this.Line);
                ZombieManage.Instance.LoadZombieMess(obj, 8, this.Line + pos.x, this.Speed());
                obj.transform.position = this.transform.position + new Vector3(pos.y * 2, pos.x * 2, 0);
                if (this.bufDetail.GetKeyWordBuf(KeyWordBuf.MindControl) != null)
                {
                    obj.GetComponent<BattleUnitModel>().bufDetail.AddKeyWordBuf(KeyWordBuf.MindControl, 1);
                }
                num++;
            }
        }
        this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, num).Maxstack += num * 2;
        base.AnimSummon();
    }
}
