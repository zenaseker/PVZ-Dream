using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class BattleControl_OneTwoThree : BattleControlBase
{
    public override void OnLevelStart()
    {
        HandManage.Instance.FastCellPlant(Vector2Int.zero, 0);
        GameObject plant = GameObject.Find("PlantManage").transform.GetChild(0).gameObject;
        GameObject.Destroy(plant.GetComponent<PlantBase>());
        plant.AddComponent<Plant_PeaByOTT>();
        PlantBase plantBase = plant.GetComponent<Plant_PeaByOTT>();//开始录入植物信息
        plantBase.Init(Vector2Int.zero, Attribute.Instance.GetPlantInfo(0));
        plantBase.HP = 10;
        plant.transform.rotation = Quaternion.Euler(0, 180, 0);
        DebugShow.Instance.Init("玩过123木头人吗？点击豌豆射手转头！", 5f);
    }
    public override void OnStartCreateZombie()
    {
        DebugShow.Instance.Init("每次转头有5秒冷却，且至多持续1秒！", 5f);
    }
    public override void OnCreateZombie(ZombiesBase zombie)
    {
        base.OnCreateZombie(zombie);
        int line = zombie.Line;
        Attribute.ZombieInfo card = (Attribute.ZombieInfo)zombie.unitInfo;
        GameObject z = zombie.gameObject;
        GameObject.Destroy(zombie);
        z.AddComponent<Zombie_OTT>().Init(z.GetComponent<SortingGroup>().sortingOrder, card, line,2);
    }
    public static void Stop()
    {
        List<Zombie_OTT> zombie = new List<Zombie_OTT>();
        for (int i = 0; i < GameObject.Find("ZombieManage").transform.childCount; i++)
        {
            Zombie_OTT j = GameObject.Find("ZombieManage").transform.GetChild(i).GetComponent<Zombie_OTT>();
            j.CheckStop(true);
            if (!j.Stop)
            {
                zombie.Add(j);
            }
        }
        foreach(Zombie_OTT zombie_OTT in zombie)
        {
            zombie_OTT.Die();
        }
    }
}
