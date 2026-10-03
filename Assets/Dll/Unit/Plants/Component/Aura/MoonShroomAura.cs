using System.Collections.Generic;
using UnityEngine;

public class MoonShroomAura : AuraBase
{
    public override void Init(ComponentDetail detail)
    {
        base.Area = new List<Vector2Int> { detail._owner.XY };
        base.Init(detail);
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_MoonShroom(map, PlantPosType.Default);
    }
    public class MapBuf_MoonShroom : MapMeshUnitBuf
    {
        public MapBuf_MoonShroom(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override void Init()
        {
            base.Init();
            foreach (PlantBase plantBase in this.mesh.GetPlants())
            {
                if (plantBase.unitInfo.DreamElement.Contains(Attribute.DreamElement.Dark)) plantBase.component._enchantment.Add(new MoonShroomEnchantment());
            }
        }
        public override void OnCellPlant(PlantBase plant)
        {
            base.OnCellPlant(plant);
            if (plant.unitInfo.DreamElement.Contains(Attribute.DreamElement.Dark)) plant.component._enchantment.Add(new MoonShroomEnchantment());
        }
        public override void OnPlantDestory(PlantBase plant)
        {
            base.OnPlantDestory(plant);
            foreach (PlantBase plantBase in this.mesh.GetPlants())
            {
                plantBase.component._enchantment.Find(x => x is MoonShroomEnchantment)?.Destory();
            }
        }
    }
}