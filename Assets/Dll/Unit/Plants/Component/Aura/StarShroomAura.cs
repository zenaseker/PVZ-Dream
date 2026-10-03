
using UnityEngine;

public class StarShroomAura : AuraBase
{
    public override void Init(ComponentDetail detail)
    {
        Area = MapManage.Instance.GetEffectiveRange(detail._owner.XY, 0.5f);
        base.Init(detail);
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_StarShroom(map, PlantPosType.Default);
    }

    public class MapBuf_StarShroom : MapMeshUnitBuf
    {
        public MapBuf_StarShroom(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }

        public override void Init()
        {
            base.Init();
            foreach (PlantBase plantBase in this.mesh.GetPlants())
            {
                if (plantBase.unitInfo.DreamElement.Contains(Attribute.DreamElement.Light))
                {
                    plantBase.bufDetail.AddBuf(new BattleUnitBuf_StarShroomSpeed());
                }
                if (((Attribute.PlantInfo)plantBase.unitInfo).Plantstic == Plantstics.Shroom)
                {
                    plantBase.bufDetail.AddBuf(new BattleUnitBuf_StarShroomHP());
                }
            }
        }
        public override void OnCellPlant(PlantBase plant)
        {
            base.OnCellPlant(plant);
            if (plant.unitInfo.DreamElement.Contains(Attribute.DreamElement.Light))
            {
                plant.bufDetail.AddBuf(new BattleUnitBuf_StarShroomSpeed());
            }
            if (((Attribute.PlantInfo)plant.unitInfo).Plantstic == Plantstics.Shroom)
            {
                plant.bufDetail.AddBuf(new BattleUnitBuf_StarShroomHP());
            }
        }
        public class BattleUnitBuf_StarShroomSpeed : BattleUnitBuf
        {
            public override float SpeedChange()
            {
                return 1.1f;
            }
        }
        public class BattleUnitBuf_StarShroomHP : BattleUnitBuf
        {
            public override int MaxHp()
            {
                return (int)(_owner.unitInfo.HP * 0.1f);
            }
        }
    }
}