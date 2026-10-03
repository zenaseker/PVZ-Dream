using System.Collections.Generic;
using UnityEngine;
public class AuraBase : ComponentBase
{
    public ComponentDetail _detail;
    protected List<Vector2Int> Area = new List<Vector2Int>();
    protected Dictionary<Vector2Int, MapMeshUnitBuf> buflist = new Dictionary<Vector2Int, MapMeshUnitBuf>();
    public virtual void Init(ComponentDetail detail)
    {
        _detail = detail;
        buflist = new Dictionary<Vector2Int, MapMeshUnitBuf>();
        foreach (Vector2Int vector in Area)
        {
            MapMeshUnitBuf buf = MapBuf(MapManage.Instance.meshPlants[vector.x, vector.y]);
            buflist.Add(vector, buf);
            MapManage.Instance.meshPlants[vector.x, vector.y].AddMapBuf(buf,1);
        }
    }

    public virtual void Destory()
    {
        foreach (Vector2Int vector in Area)
        {
            MapManage.Instance.meshPlants[vector.x, vector.y].mapbuflist.Remove(buflist[vector]);
        }
    }
    public virtual MapMeshUnitBuf MapBuf(MapMeshPlant map)
    {
        return null;
    }
}
