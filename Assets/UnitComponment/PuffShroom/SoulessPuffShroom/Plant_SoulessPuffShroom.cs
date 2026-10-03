using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class Plant_SoulessPuffShroom : Plant_PuffShroom
{
    public class BattleUnitBuf_SoulessPossess : BuffManage.BattleUnitBuf_Possess
    {
        public override PossessType possessType
        {
            get
            {
                return PossessType.PuffShroom;
            }
        }
        public override int BrokenNum => 2;
        public override void OnAttack(DamageObject damageObject)
        {
            base.OnAttack(damageObject);
            WakeUpNum++;
        }
        public override void OnGetBullet(IEnchantment enchantment)
        {
            base.OnGetBullet(enchantment);
            if (WakeUpNum >= BrokenNum)
            {
                WakeUpNum = 0;
                enchantment.Action += Hit;
            }
        }
        public override void Hit(BattleUnitModel battleUnitModel, int dmg)
        {
            GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Default", battleUnitModel.transform.position + Vector3.up + Vector3.left * 0.7f);
            gameObject.GetComponent<BulletBase>().Init(new Vector2(-5.5f, 5.5f), -1, UnitFaction.Plant, 20);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 125f);

            GameObject gameObject2 = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Default", battleUnitModel.transform.position + Vector3.up * 0.7f + Vector3.left * 0.7f);
            gameObject2.GetComponent<BulletBase>().Init(new Vector2(-5.5f, -5.5f), -1, UnitFaction.Plant, 20);
            gameObject2.transform.rotation = Quaternion.Euler(0, 0, -125f);

            GameObject gameObject3 = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Default", battleUnitModel.transform.position + Vector3.up * 0.7f + Vector3.right * 0.7f);
            gameObject3.GetComponent<BulletBase>().Init(new Vector2(5.5f, -5.5f), -1, UnitFaction.Plant, 20);
            gameObject3.transform.rotation = Quaternion.Euler(0, 0, -45f);

            GameObject gameObject4 = PoolManage.Instance.GetPoolGameObject("Bullet", "PuffShroom_Default", battleUnitModel.transform.position + Vector3.up + Vector3.right * 0.7f);
            gameObject4.GetComponent<BulletBase>().Init(new Vector2(5.5f, 5.5f), -1, UnitFaction.Plant, 20);
            gameObject4.transform.rotation = Quaternion.Euler(0, 0, 45f);
        }
    }
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        MapManage.Instance.meshPlants[xy.x, xy.y].meshplantchange += OnCellPlant;
        OnCellPlant(MapManage.Instance.meshPlants[XY.x, XY.y].GetPlant(PlantPosType.Default), null);
    }
    public override void OnChangeMesh()
    {
        base.OnChangeMesh();
        OnCellPlant(MapManage.Instance.meshPlants[XY.x, XY.y].GetPlant(PlantPosType.Default), null);
    }
    public void OnCellPlant(PlantBase plant,PlantBase orginplant)
    {
        if (posType == PlantPosType.Little && plant != null && plant.posType == PlantPosType.Default && ((Attribute.PlantInfo)plant.unitInfo).Planttype == PlantType.Attack)
        {
            if (plant.bufDetail.GetBufList().Find(x => x is BattleUnitBuf_SoulessPossess) == null)
            {
                if (plant.bufDetail.GetKeyWordBuf(KeyWordBuf.Possess) != null)
                {
                    plant.bufDetail.RemoveBuf(x => x.KeyWordBuf == KeyWordBuf.Possess);
                }
                plant.bufDetail.AddBuf(new BattleUnitBuf_SoulessPossess());
            }
            this.Die();
        }
    }
    public override void Die()
    {
        base.Die();
        MapManage.Instance.meshPlants[XY.x, XY.y].meshplantchange -= OnCellPlant;
    }
}
