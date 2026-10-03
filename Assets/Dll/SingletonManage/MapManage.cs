
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static Attribute;
using static DefaultMapBuff;
using static MapManage;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public enum MapActionType
{
    RangeCherry,//有范围樱桃爆炸
    RangeLight,//有范围光爆炸
    RangeSnow,//有范围冰冻
    LineFire,//单行辣椒
    ScreenLight,//全屏光爆炸
    ScreenSnow,//全屏冰冻
}
/// <summary>
/// 射线方向
/// </summary>
public enum RayDirection
{
    zero,//原点
    left,//左
    right,//右
    up,//上
    down,//下
    line,//行
    column,//列
    all//行+列
}

public class MapManage : Singleton<MapManage>
{
    public GameObject MeshBoxList;
    /// <summary>
    /// 极限坐标
    /// </summary>
    public Vector2Int meshxy = new Vector2Int();
    /// <summary>
    /// 坐标数组
    /// </summary>
    public Vector3[,] meshpos;//侧方x+0.6，y-0.5
    /// <summary>
    /// 瓦片模块数组
    /// </summary>
    public MapMeshPlant[,] meshPlants;
    public Action<Vector2Int, MapActionType> AreaChack;


    public void Update()
    {
        foreach (MapMeshPlant mesh in meshPlants)
        {
            mesh.Update();
        }
    }
    public List<Vector2Int> GetEffectiveAll()
    {
        List<Vector2Int> range = new List<Vector2Int>();
        foreach (MapMeshPlant v in meshPlants)
        {
            range.Add(v.xy);
        }
        return range;
    }
    public List<Vector2Int> GetEffectiveLine(Vector2Int vector2, RayDirection direction, int length = 999)
    {
        List<Vector2Int> range = new List<Vector2Int>();
        foreach (MapMeshPlant v in meshPlants)
        {
            if (InRay(vector2, v.xy, direction) && UnityEngine.Vector2.Distance(vector2, v.xy) <= length)
            {
                range.Add(v.xy);
            }
        }
        return range;
    }
    private bool InRay(Vector2Int vector, Vector2Int vector2, RayDirection direction)
    {
        if (vector.x != vector2.x)//横坐标不相等
        {
            if (vector.y == vector2.y)//纵坐标相等
            {
                if (direction == RayDirection.column || direction == RayDirection.all)
                {
                    return true;
                }
                else if (direction == RayDirection.down && vector.x > vector2.x)//纵坐标小于原点
                {
                    return true;
                }
                else if (direction == RayDirection.up && vector.x < vector2.x)//纵坐标大于原点
                {
                    return true;
                }
            }
            return false;
        }
        else if (vector.y != vector2.y)//纵坐标不相等
        {
            if (vector.x == vector2.x)//横坐标相等
            {
                if (direction == RayDirection.line || direction == RayDirection.all)
                {
                    return true;
                }
                else if (direction == RayDirection.left && vector.y > vector2.y)//横坐标小于原点
                {
                    return true;
                }
                else if (direction == RayDirection.right && vector.y < vector2.y)//横坐标大于原点
                {
                    return true;
                }
            }
            return false;
        }
        else if (direction == RayDirection.zero || direction == RayDirection.all)
        {
            return true;
        }
        return false;
    }
    public List<Vector2Int> GetEffectiveRange(Vector2Int vector2, float radius)
    {
        List<Vector2Int> range = new List<Vector2Int>();
        foreach (MapMeshPlant v in meshPlants)
        {
            if (Vector2Int.Distance(v.xy, vector2) < radius)
            {
                range.Add(v.xy);
            }
        }
        return range;
    }

    public bool InOneLine(Vector3 vector3, int x)
    {
        if (Mathf.Abs(vector3.y - meshpos[x, 0].y) < 0.1f)
        {
            return true;
        }
        return false;
    }
    public List<PlantBase> GetAllPlantByPosType(PlantPosType plantPosType)
    {
        List<PlantBase> plants = new List<PlantBase>();
        foreach(MapMeshPlant mapMeshPlant in meshPlants)
        {
            PlantBase plant = mapMeshPlant.GetPlant(plantPosType);
            if (plant != null)
            {
                plants.Add(plant);
            }
        }
        return plants;
    }

    public void OnEnable()
    {
        MeshBoxList = Resources.Load<GameObject>("Prefabs/PlantMesh/MeshBoxList_" + Attribute.Instance.levelAttribute.plantmesh);
        meshxy = MeshBoxList.GetComponent<MeshBoxList>().list;
        meshpos = new Vector3[meshxy.x, meshxy.y];
        meshPlants = new MapMeshPlant[meshxy.x, meshxy.y];
        AreaChack = null;
        for(int i = 0; i < MeshBoxList.transform.childCount; i++)
        {
            Vector2Int vector = MeshBoxList.transform.GetChild(i).GetComponent<GridNode>().vector;
            meshpos[vector.x, vector.y] = MeshBoxList.transform.GetChild(i).transform.position;
            meshPlants[vector.x, vector.y] = new MapMeshPlant(vector.x, vector.y, MeshBoxList.transform.GetChild(i).GetComponent<GridNode>().gridKey);
        }
        MeshBoxList = null;
    }
    public void CellTombston()
    {
        if (Attribute.Instance.levelAttribute.Tombston > 0 && meshxy.y > 4 && meshxy.x > 4)
        {
            List<Vector2Int> Cemeterys = new List<Vector2Int>();
            do
            {
                Vector2Int vector = new Vector2Int(Random.Range(0, meshxy.x), Random.Range(6, meshxy.y));
                if (!Cemeterys.Contains(vector))
                {
                    Cemeterys.Add(vector);
                }
            } while (Cemeterys.Count < Attribute.Instance.levelAttribute.Tombston);
            foreach (Vector2Int vector1 in Cemeterys)
            {
                meshPlants[vector1.x, vector1.y].tombston = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Tombston")).GetComponent<Tombston>();
                meshPlants[vector1.x, vector1.y].tombston.pos = vector1;
                meshPlants[vector1.x, vector1.y].tombston.transform.position = new Vector3(meshpos[vector1.x, vector1.y].x, meshpos[vector1.x, vector1.y].y, 0);
            }
        }
    }
    public bool CanCell(int x, int y,PlantPosType plantPosType)
    {
        if ((this.meshPlants[x, y].tombston != null || this.meshPlants[x, y].mapbuflist.Find(x => x is MapMeshUnitBuf_DoomCrater) != null)
            && plantPosType != PlantPosType.Top) return false;

        if (plantPosType == PlantPosType.Little)
        {
            return this.meshPlants[x, y].GetPlant(PlantPosType.Default) == null ||  this.meshPlants[x, y].GetPlant(PlantPosType.Little) == null;
        }
        else if (plantPosType == PlantPosType.Default)
        {
            return this.meshPlants[x, y].GetPlant(PlantPosType.Default) == null || 
                (this.meshPlants[x, y].GetPlant(PlantPosType.Default) != null && ((Attribute.PlantInfo)this.meshPlants[x, y].GetPlant(PlantPosType.Default).unitInfo).Plantpostype == PlantPosType.Little && this.meshPlants[x, y].GetPlant(PlantPosType.Little) == null);
        }
        return this.meshPlants[x, y].GetPlant(plantPosType) == null;
    }//可种植
    public bool CanFusion(int x,int y, PlantPosType plantPosType, Attribute.PlantInfo plant)
    {
        return plant != null && this.meshPlants[x, y].GetPlant(plantPosType) != null && Attribute.Instance.GetFusion(plant.ID, this.meshPlants[x, y].GetPlant(plantPosType).unitInfo.ID) != -1;
    }//可融合
    public bool CanDye(int x, int y, PlantPosType plantPosType, Attribute.PlantInfo plant)
    {
        if (this.meshPlants[x, y].GetPlant(plantPosType) == null) return false;
        if (plant.DreamElement.Contains(DreamElement.Dream)) return false;
        if (plant.DreamElement.Contains(DreamElement.Default)) return false;
        if (plant.DreamElement == this.meshPlants[x, y].GetPlant(plantPosType).unitInfo.DreamElement) return false;
        if (plant.ID % 100 != this.meshPlants[x, y].GetPlant(plantPosType).unitInfo.ID % 100) return false;
        HashSet<DreamElement> set1 = new HashSet<DreamElement>(plant.DreamElement);
        return this.meshPlants[x, y].GetPlant(plantPosType).unitInfo.DreamElement.All(x => x == DreamElement.Default || set1.Contains(x));
    }//可染色
    public void CheckPlantCellPos(int x, int y, PlantBase plant, PlantPosType plantPosType,out PlantPosType LastPosType)
    {
        LastPosType = plantPosType;
        if (plantPosType == PlantPosType.Little)
        {
            if (this.meshPlants[x, y].GetPlant(PlantPosType.Default) == null)
            {
                this.meshPlants[x, y].AddPlant(plant, PlantPosType.Default);
                LastPosType = PlantPosType.Default;
            }
            else if (this.meshPlants[x, y].GetPlant(PlantPosType.Little) == null)
            {
                this.meshPlants[x, y].AddPlant(plant, PlantPosType.Little);
                LastPosType = PlantPosType.Little;
            }
            else
            {
                Debug.Log("你在种什么？");
            }
            return;
        }
        if (plantPosType == PlantPosType.Default && this.meshPlants[x, y].GetPlant(PlantPosType.Default) != null
            && ((Attribute.PlantInfo)this.meshPlants[x, y].GetPlant(PlantPosType.Default).unitInfo).Plantpostype == PlantPosType.Little && this.meshPlants[x, y].GetPlant(PlantPosType.Little) == null)
        {
            PlantBase littleplant = this.meshPlants[x, y].GetPlant(PlantPosType.Default);
            this.meshPlants[x, y].AddPlant(littleplant, PlantPosType.Little);
            this.meshPlants[x, y].AddPlant(plant, PlantPosType.Default);
            littleplant.posType = PlantPosType.Little;
            littleplant.OnChangeMesh();
            LastPosType = PlantPosType.Default;
            return;
        }
        CellPlant(x, y, plant, plantPosType);
    }
    public void CellPlant(int x, int y, PlantBase plant, PlantPosType plantPosType)
    {
        this.meshPlants[x, y].AddPlant(plant, plantPosType);
    }
    public void DestoryCellPlant(int x, int y, PlantPosType plantPosType)
    {
        this.meshPlants[x, y].AddPlant(null, plantPosType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            if (collision.gameObject.GetComponent<ZombiesBase>().Reverse)
            {
                collision.gameObject.GetComponent<ZombiesBase>().Destroy();
                return;
            }
            BattleManage.Instance.ZombieInHome();
        }
    }
}

public class DefaultMapBuff
{
    public class MapMeshUnitBuf_Night : MapMeshUnitBuf
    {
        public MapMeshUnitBuf_Night(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }

        public override float Product(PlantBase plant)
        {
            if (plant.unitInfo.ID % 100 == 9)
            {
                return 1;
            }
            return 2;
        }
    }
    public class MapMeshUnitBuf_DoomCrater : MapMeshUnitBuf
    {
        float starttime;
        float time;
        GameObject crater;
        public MapMeshUnitBuf_DoomCrater(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override void Update()
        {
            base.Update();
            time -= Time.deltaTime;
            if (time <= starttime / 2)
            {
                crater.transform.GetChild(1).gameObject.SetActive(false);
                if (time <= 0)
                {
                    crater.transform.GetChild(1).gameObject.SetActive(true);
                    PoolManage.Instance.PushGameObject(crater.name, crater);
                    this.Destory();
                }
            }
        }
        public override void Init()
        {
            base.Init();
            time = starttime = 180f;
            switch (mesh.key)
            {
                case GridKey.Day:
                    crater = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Carter_Day", MapManage.Instance.meshpos[mesh.xy.x, mesh.xy.y]);
                    break;
                case GridKey.Night:
                    crater = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Carter_Night", MapManage.Instance.meshpos[mesh.xy.x, mesh.xy.y]);
                    break;
                case GridKey.Pool:
                    break;
                case GridKey.NightPool:
                    break;
                case GridKey.Roof:
                    break;
            }
        }
    }
}

public class MapMeshPlant
{
    public MapMeshPlant(int x,int y, GridKey key)
    {
        this.xy = new Vector2Int(x, y);
        this.key = key;
        switch (this.key)
        {
            case GridKey.Night:
                AddMapBuf(new DefaultMapBuff.MapMeshUnitBuf_Night(this, PlantPosType.All), 1);
                break;
            case GridKey.Pool:
                break;
            case GridKey.NightPool:
                break;
            case GridKey.Roof:
                break;
        }
    }
    public Vector2Int xy = new Vector2Int();
    public GridKey key;
    public Tombston tombston = null;
    private PlantBase _DefaultPlant = null;//中
    private PlantBase _BasePlant = null;//下
    private PlantBase _ShellPlant = null;//壳
    private PlantBase _TopPlant = null;//顶
    private PlantBase _LittlePlant = null;//小
    public Action<PlantBase, PlantBase> meshplantchange;
    public List<MapMeshUnitBuf> mapbuflist = new List<MapMeshUnitBuf>();
    public List<MapMeshUnitBuf> removemapbuflist = new List<MapMeshUnitBuf>();

    public void Update()
    {
        if (removemapbuflist.Count > 0)
        {
            foreach (MapMeshUnitBuf buf1 in removemapbuflist)
            {
                mapbuflist.Remove(buf1);
            }
            removemapbuflist.Clear();
        }
        foreach (MapMeshUnitBuf buf in mapbuflist)
        {
            buf.Update();
        }
    }
    public void AddMapBuf(MapMeshUnitBuf mapbuf,int stack)
    {
        if (mapbuflist.Find(x => x.GetType() == mapbuf.GetType()) != null && stack > 0)
        {
            mapbuflist.Find(x => x.GetType() == mapbuf.GetType()).AddStack(stack);
            return;
        } 
        mapbuflist.Add(mapbuf);
        mapbuf.AddStack(stack);
        mapbuf.Init();
    }
    public bool HasPlant()
    {
        if (_DefaultPlant != null)
        {
            return true;
        }
        if (_BasePlant != null)
        {
            return true;
        }
        if (_ShellPlant != null)
        {
            return true;
        }
        if (_TopPlant != null)
        {
            return true;
        }
        return false;
    }

    public List<MapMeshUnitBuf> GetMapMeshUnitBufs(PlantPosType plantpostype)
    {
        List<MapMeshUnitBuf> buffs = new List<MapMeshUnitBuf>();
        foreach(MapMeshUnitBuf buf in mapbuflist)
        {
            if (buf.EffectiveUnit == plantpostype || buf.EffectiveUnit == PlantPosType.All)
            {
                buffs.Add(buf);
            }
        }
        return buffs;
    }
    public bool OnTakeDamage(DamageObject damage)
    {
        foreach(MapMeshUnitBuf buf in mapbuflist)
        {
            if(buf.OnTakeDamage(damage))
            {
                return true;
            }
        }
        return false;
    }
    public PlantBase GetPlant(PlantPosType plantPosType)
    {
        switch (plantPosType)
        {
            case PlantPosType.Base:
                return _BasePlant;
            case PlantPosType.Shell:
                return _ShellPlant;
            case PlantPosType.Top:
                return _TopPlant;
            case PlantPosType.Little:
                return _LittlePlant;
        }
        return _DefaultPlant;
    }
    public List<PlantBase> GetPlants()
    {
        List<PlantBase > list = new List<PlantBase>();
        if (_DefaultPlant != null)
        {
            list.Add(_DefaultPlant);
        }
        if (_ShellPlant != null)
        {
            list.Add(_ShellPlant);
        }
        if (_TopPlant != null)
        {
            list.Add(_TopPlant);
        }
        if (_LittlePlant != null)
        {
            list.Add(_LittlePlant);
        }
        if (_BasePlant != null)
        {
            list.Add(_BasePlant);
        }
        return list;

    }
    public void AddPlant(PlantBase plant,PlantPosType plantPosType)
    {
        if (GetPlant(plantPosType) != null)
        {
            foreach (MapMeshUnitBuf mapMeshUnitBuf in this.GetMapMeshUnitBufs(plantPosType))
            {
                mapMeshUnitBuf.OnPlantDestory(GetPlant(plantPosType));
            }
        }
        PlantBase originplant = GetPlant(plantPosType);
        if (plantPosType == PlantPosType.Base)
        {
            _BasePlant = plant;
        }
        else if (plantPosType == PlantPosType.Shell)
        {
            _ShellPlant = plant;
        }
        else if (plantPosType == PlantPosType.Top)
        {
            if (plant != null)
            {
                plant.GetComponent<SortingGroup>().sortingOrder += 100;
            }
            _TopPlant = plant;
        }
        else if (plantPosType == PlantPosType.Little)
        {
            if (plant != null)
            {
                plant.GetComponent<SortingGroup>().sortingOrder += 75;
            }
            _LittlePlant = plant;
            if (_LittlePlant != null)
            {
                _LittlePlant.transform.position += new Vector3(0.6f, -0.5f, -1f);
            }
        }
        else
        {
            _DefaultPlant = plant;
        }
        meshplantchange?.Invoke(plant, originplant);
        if (GetPlant(plantPosType) != null)
        {
            foreach (MapMeshUnitBuf mapMeshUnitBuf in this.GetMapMeshUnitBufs(plantPosType))
            {
                mapMeshUnitBuf.OnCellPlant(plant);
            }
        }
    }
}