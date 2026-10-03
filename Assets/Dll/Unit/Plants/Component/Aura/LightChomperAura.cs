using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms.VisualStyles;
using UnityEngine;

public class LightChomperAura : AuraBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = MapManage.Instance.GetEffectiveLine(detail._owner.XY, RayDirection.left, 3);
        base.Init(detail);
        this._detail._owner.bufDetail.AddBuf(new BattleUnitBuf_LightChomperowner());
    }
    public void GetSunCool(int num)
    {
        this._detail._owner.bufDetail.AddBuf(new BattleUnitBuf_LightChomperowner(),num / 2);
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_LightChomper(map,PlantPosType.All,this);
    }
    public class BattleUnitBuf_LightChomperowner : BattleUnitBuf
    {
        public override void AddStack(int stack, float countDown)
        {
            base.AddStack(stack, countDown);
            if (this.stack > 20)
            {
                this.stack = 20;
            }
            if (this._owner is Plant_LightChomper)
            {
                ((Plant_LightChomper)this._owner).CheckSunCool(this.stack);
            }
        }
        public override float EatingTime()
        {
            return this.stack;
        }
        public override void OnAttack(DamageObject damageObject)
        {
            base.OnAttack(damageObject);
            this.stack = 0;
        }
    }

    public class MapBuf_LightChomper : MapMeshUnitBuf
    {
        public LightChomperAura origin;
        public MapBuf_LightChomper(MapMeshPlant mesh, PlantPosType effectiveUnit, LightChomperAura origin) : base(mesh, effectiveUnit)
        {
            this.origin = origin;
        }
        public override void Init()
        {
            base.Init();
            foreach (PlantBase plant in this.mesh.GetPlants())
            {
                if (plant != null && ((Attribute.PlantInfo)plant.unitInfo).Planttype == PlantType.Production)
                {
                    plant.bufDetail.AddBuf(new BattleUnitBuf_LightChomper(origin));
                }
            }
        }
        public override void OnCellPlant(PlantBase plant)
        {
            base.OnCellPlant(plant);
            if (plant != null && ((Attribute.PlantInfo)plant.unitInfo).Planttype == PlantType.Production)
            {
                plant.bufDetail.AddBuf(new BattleUnitBuf_LightChomper(origin));
            }
        }
    }
    public class BattleUnitBuf_LightChomper : BattleUnitBuf
    {
        LightChomperAura _plant = null;
        public BattleUnitBuf_LightChomper(LightChomperAura plant)
        {
            this._plant = plant;
        }
        public override void OnProduct(GameObject sun)
        {
            base.OnProduct(sun);
            if (_plant != null && _plant._detail._owner.bufDetail.GetBufList().Find(x=>x is BattleUnitBuf_LightChomperowner)?.stack < 20)
            {
                DOTween.Kill(sun, true);
                sun.transform.DOPath(new UnityEngine.Vector3[] { sun.transform.position, _plant._detail._owner.transform.position }, 0.6f).SetEase(Ease.Linear).onComplete += () =>
                {
                    _plant?.GetSunCool(sun.GetComponent<Sun>().num);
                    PoolManage.Instance.PushGameObject(sun.name, sun);
                };
            }
        }
    }
}
