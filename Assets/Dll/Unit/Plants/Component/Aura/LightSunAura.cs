using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class LightSunAura : AuraBase
{
    GameObject lightarea = null;
    public override void Init(ComponentDetail detail)
    {
        Area = MapManage.Instance.GetEffectiveRange(detail._owner.XY, 1.5f);
        base.Init(detail);
        lightarea = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "SunLightArea", this._detail._owner.transform.position);
    }
    public override void Destory()
    {
        PoolManage.Instance.PushGameObject(lightarea.name, lightarea);
        base.Destory();
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_LightSunflower(map, PlantPosType.Default);
    }

    public class MapBuf_LightSunflower : MapMeshUnitBuf
    {
        public MapBuf_LightSunflower(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override void AddStack(int stack)
        {
            base.AddStack(stack);
            if (this.stack <= 0)
            {
                this.Destory();
            }
            if (this.mesh.GetPlant(this.EffectiveUnit) != null)
            {
                this.mesh.GetPlant(this.EffectiveUnit).GetProductTime();
            }
        }
        public override float Product(PlantBase plant)
        {
            return 1 - 0.05f * this.stack;
        }
    }
}
