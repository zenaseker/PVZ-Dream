
using UnityEngine;

public class DarkFrostAura : AuraBase
{
    public override void Init(ComponentDetail detail)
    {
        Area = MapManage.Instance.GetEffectiveRange(detail._owner.XY, 2.5f);
        base.Init(detail);
    }
    public override MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return new MapBuf_DarkFrost(map, PlantPosType.All);
    }

    public class MapBuf_DarkFrost : MapMeshUnitBuf
    {
        public MapBuf_DarkFrost(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override void AddStack(int stack)
        {
            base.AddStack(stack);
            if (this.stack <= 0)
            {
                this.Destory();
            }
        }
        public override bool OnTakeDamage(DamageObject damageObject)
        {
            damageObject.Damage = (int)(damageObject.Damage * (1 - 0.02f * this.stack));
            return base.OnTakeDamage(damageObject);
        }
    }
}
