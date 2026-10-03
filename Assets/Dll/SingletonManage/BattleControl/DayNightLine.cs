
using System.Numerics;
using UnityEngine;
using static Attribute;
using static BuffManage;

public class BattleControl_DayNightLine : BattleControlBase
{
    float movetime = 2.61f;
    bool ischange = false;//×î×ó£º-14£¬×îÓÒ£º14
    public override void OnUpdate()
    {
        base.OnUpdate();
        movetime += Time.deltaTime / 30;
        if (movetime > 3.67f)
        {
            movetime = 2.61f;
            ischange = !ischange;
            Map.GetComponent<SpriteRenderer>().material.SetFloat("IsChange", ischange?1:0);
        }
        Map.GetComponent<SpriteRenderer>().material.SetFloat("TimeMove", movetime);
        int num = -1;
        for (int i = 0; i < MapManage.Instance.meshxy.y; i++)
        {
            if (MapManage.Instance.meshpos[0,i].x < (-14 + 28 / 1.06 * (movetime - 2.61f)))
            {
                num = i;
            }
        }
        foreach(MapMeshPlant mapMeshPlant in MapManage.Instance.meshPlants)
        {
            if (mapMeshPlant.xy.y <= num)
            {
                if (!ischange)
                {
                    mapMeshPlant.mapbuflist.Find(x => x is DefaultMapBuff.MapMeshUnitBuf_Night)?.Destory();
                    if (mapMeshPlant.GetPlants().FindAll(x => ((Attribute.PlantInfo)x.unitInfo).Plantstic == Plantstics.Shroom).Count > 0)
                    {
                        foreach(PlantBase plant in (mapMeshPlant.GetPlants().FindAll(x => ((Attribute.PlantInfo)x.unitInfo).Plantstic == Plantstics.Shroom)))
                        {
                            plant.bufDetail.buflist.Find(x=>x is BattleUnitBuf_PlantSleep)?.Destory();
                            plant._CanAttack = true;
                        }
                    }
                }
                else
                {
                    mapMeshPlant.AddMapBuf(new DefaultMapBuff.MapMeshUnitBuf_Night(mapMeshPlant, PlantPosType.All), 1);
                    if (mapMeshPlant.GetPlants().FindAll(x => ((Attribute.PlantInfo)x.unitInfo).Plantstic == Plantstics.Shroom).Count > 0)
                    {
                        foreach (PlantBase plant in (mapMeshPlant.GetPlants().FindAll(x => ((Attribute.PlantInfo)x.unitInfo).Plantstic == Plantstics.Shroom)))
                        {
                            plant.bufDetail.AddBuf(new BuffManage.BattleUnitBuf_PlantSleep(), 1);
                            plant._CanAttack = false;
                        }
                    }
                }
            }
        }
    }
}
