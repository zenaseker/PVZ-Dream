using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Plant_SoulGravebuster : Plant_Gravebuster
{
    public void Eat2()
    {
        if (MapManage.Instance.meshPlants[this.XY.x, this.XY.y].tombston != null)
        {
            CreateZombie();
            CreateZombie();
            CreateZombie();
            Eat();
            return;
        }
        List<BattleUnitModel> unit = new List<BattleUnitModel>();
        unit.AddRange(MapManage.Instance.meshPlants[this.XY.x, this.XY.y].GetPlants().FindAll(x => x.bufDetail.GetKeyWordBuf(KeyWordBuf.Possess) != null));
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 1))
        {
            if (monster.gameObject.tag == "Zombie" && !monster.gameObject.GetComponent<ZombiesBase>().Reverse && !monster.gameObject.GetComponent<ZombiesBase>().losehead)
            {
                if (monster.gameObject.GetComponent<ZombiesBase>().bufDetail.GetKeyWordBuf(KeyWordBuf.Possess) != null)
                {
                    unit.Add(monster.gameObject.GetComponent<ZombiesBase>());
                }
            }
        }
        if (unit.Count > 0)
        {
            int num = 0;
            foreach(BattleUnitModel battleUnitModel in unit)
            {
                battleUnitModel.bufDetail.RemoveBuf(x => x.KeyWordBuf == KeyWordBuf.Possess);
                num++;
            }
            while(num > 0)
            {
                CreateZombie();
                num--;
            }
        }
        this.Die();
    }
    void CreateZombie()
    {
        GameObject obj = ZombieManage.Instance.InitZombie(0, this.XY.x);
        ZombieManage.Instance.LoadZombieMess(obj, 0, this.XY.x);
        obj.transform.position = MapManage.Instance.meshpos[this.XY.x,this.XY.y]+ new Vector3(Random.Range(-0.2f, 0.2f), -0.8f,0);
        obj.GetComponent<ZombiesBase>().ZombieUp();
        obj.GetComponent<BattleUnitModel>().bufDetail.AddKeyWordBuf(KeyWordBuf.MindControl, 1);
        obj.GetComponent<BattleUnitModel>().bufDetail.AddBuf(new BattleUnitBuf_SoulGravebuster(), 1);
    }
    public class BattleUnitBuf_SoulGravebuster : BuffManage.BattleUnitBuf_Possess
    {
        public override PossessType possessType
        {
            get
            {
                return PossessType.Gravebuster;
            }
        }
        public override int BrokenNum => 2;
        public override void OnAttack(DamageObject damageObject)
        {
            base.OnAttack(damageObject);
            WakeUpNum++;
            if (WakeUpNum >= 2)
            {
                WakeUpNum = 0;
                damageObject.Damage *= 2;
                damageObject.DamageElement = DamageElement.Soul;
            }
        }

    }
}
