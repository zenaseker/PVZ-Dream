using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class AuroraAura : AuraBase
{
    public override void Init(ComponentDetail detail)
    {
        Area = MapManage.Instance.GetEffectiveAll();
        base.Init(detail);
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_Aurora(map,PlantPosType.Default);
    }

    public class MapBuf_Aurora : MapMeshUnitBuf
    {
        public MapBuf_Aurora(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override void AddStack(int stack)
        {
            base.AddStack(stack);
            if (stack > 0)
            {
                if (this.stack > 6)
                {
                    this.stack = 6;
                }
            }
            else
            {
                if (this.stack <= 0)
                {
                    this.Destory();
                }
            }
            if (this.mesh.GetPlant(this.EffectiveUnit) != null)
            {
                this.mesh.GetPlant(this.EffectiveUnit).GetProductTime();
                if (this.mesh.GetPlant(this.EffectiveUnit) is Plant_AuroraSunFlower)
                {
                    ((Plant_AuroraSunFlower)this.mesh.GetPlant(this.EffectiveUnit)).CheckAuroraNumber();
                }
            }
        }
        public override float Product(PlantBase plant)
        {
            return 1 - 0.03f * this.stack;
        }
        public override void Destory()
        {
            base.Destory();
        }
    }
}
