using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class SunMoonDoublePeaAura : AuraBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = MapManage.Instance.GetEffectiveAll();
        base.Init(detail);
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_SunMoonShroom(map, PlantPosType.Default);
    }
    public class MapBuf_SunMoonShroom : MapMeshUnitBuf
    {
        public MapBuf_SunMoonShroom(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override void Init()
        {
            base.Init();
            foreach (PlantBase plantBase in this.mesh.GetPlants())
            {
                plantBase.bufDetail.AddBuf(new BattleUnitBuf_SunMoon(), 1);
            }
        }
        public override void OnCellPlant(PlantBase plant)
        {
            base.OnCellPlant(plant);
            plant.bufDetail.AddBuf(new BattleUnitBuf_SunMoon(), 1);
        }
        public override void OnPlantDestory(PlantBase plant)
        {
            base.OnPlantDestory(plant);
            foreach (PlantBase plantBase in this.mesh.GetPlants())
            {
                plant.bufDetail.AddBuf(new BattleUnitBuf_SunMoon(), -1);
            }
        }
        public class BattleUnitBuf_SunMoon : BattleUnitBuf
        {
            public override float SpeedChange()
            {
                if (this._owner.unitInfo.DreamElement.Contains(Attribute.DreamElement.Light))
                {
                    if (this._owner is Plant_SunMoonDoublePea && ((Plant_SunMoonDoublePea)this._owner).InState != SunMoonState.None)
                    {
                        return base.SpeedChange();
                    }
                    return (float)(1 - 0.1 * this.stack);
                }
                if (this._owner.unitInfo.DreamElement.Contains(Attribute.DreamElement.Dark))
                {
                    return (float)(1 + 0.15 * this.stack);
                }
                return base.SpeedChange();
            }
            public override float GiveDamageChange(int dmg, DamageElement damageType)
            {
                if (this._owner.unitInfo.DreamElement.Contains(Attribute.DreamElement.Light))
                {
                    return (float)(1 + 0.15 * this.stack);
                }
                if (this._owner.unitInfo.DreamElement.Contains(Attribute.DreamElement.Dark))
                {
                    if (this._owner is Plant_SunMoonDoublePea && ((Plant_SunMoonDoublePea)this._owner).InState != SunMoonState.None)
                    {
                        return base.SpeedChange();
                    }
                    return (float)(1 - 0.1 * this.stack);
                }
                return base.GiveDamageChange(dmg, damageType);
            }
        }
    }
}