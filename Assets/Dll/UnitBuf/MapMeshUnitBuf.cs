
public class MapMeshUnitBuf
{
    public MapMeshPlant mesh = null;
    public PlantPosType EffectiveUnit = PlantPosType.Default;
    public int stack = 0;
    public MapMeshUnitBuf(MapMeshPlant mesh, PlantPosType effectiveUnit)
    {
        this.mesh = mesh;
        EffectiveUnit = effectiveUnit;
    }
    public virtual void AddStack(int stack) 
    {
        this.stack += stack;
    }
    public virtual void Init() { }
    public virtual void Update() { }
    public virtual void OnCellPlant(PlantBase plant) { }
    public virtual void OnPlantDestory(PlantBase plant) { }
    public virtual void Destory()
    {
        mesh.removemapbuflist.Add(this);
    }
    public virtual bool OnTakeDamage(DamageObject damageObject) { return false; }
    public virtual float Product(PlantBase plant)
    {
        return 1;
    }
}