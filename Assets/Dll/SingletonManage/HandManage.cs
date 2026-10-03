
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 种植植物时判定
/// </summary>
public enum CellPlantType
{
    Canot,//不允许种植
    Cell,//允许种植
    Fusion,//允许融合
    Present,//礼盒
    CanDye,//可染色
}

public class HandManage : Singleton<HandManage>
{
    [Tooltip("在手中的植物")]
    public GameObject hand;
    [Tooltip("在手中的植物投影")]
    public GameObject cellhand;
    [Tooltip("植物管理实例")]
    public GameObject PlantManage;
    [Tooltip("原始卡片组件")]
    public Seed OriginCardSeed;
    [SerializeField]
    public Vector3 Vector3;
    public Vector2Int StandVector2;
    private Attribute.PlantInfo card = null;
    public List<int> cellplantid = new List<int>();
    [Tooltip("排山倒海")]
    public bool plantline = false;
    //同帧保护
    bool oneupdateprotect = false;


    public void Start()
    {
        PlantManage = GameObject.Find("PlantManage");
        hand = null;
        if (cellhand!= null)
        {
            GameObject.Destroy(cellhand);
        }
        cellhand = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/CellShadow"));
        cellhand.SetActive(false);
        OriginCardSeed = null;
        card = null;
    }

    void Update()
    {
        if (hand == null) return;
        if (Input.GetMouseButtonUp(1))
        {
            ClearHandPlant();
            return;
        }
        Vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3.z = -1;
        hand.transform.position = Vector3;
        cellhand.transform.position = GetStandWorldPoint(hand.transform.position,ref StandVector2);
        CellPlantType get = CellPlanttype(StandVector2, card.ID);
        if (get == CellPlantType.Canot)
        {
            cellhand.SetActive(false); 
            return;
        }
        cellhand.SetActive(true);
        if (oneupdateprotect)
        {
            oneupdateprotect = false;
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (plantline)
            {
                for (int i = 0; i < MapManage.Instance.meshxy.x; i++)
                {
                    Vector2Int pos2 = new Vector2Int(i, StandVector2.y);
                    CellPlant(pos2, CellPlanttype(pos2, card.ID, false),true);
                }
            }
            else
            {
                CellPlant(StandVector2, get);
            }
            CardToCool();
        }
    }

    public void ClearHandPlant()
    {
        GameObject.Destroy(hand);
        hand = null;
        OriginCardSeed.incool = Cardtype.CanUse;
        OriginCardSeed = null;
        cellhand.SetActive(false);
        MusicManage.Instance.PlayEffect("tap", 1f);
    }

    public Vector3 GetStandWorldPoint(Vector3 pos,ref Vector2Int mesh)
    {
        float min = 99f;
        Vector3 ans = new Vector3();
        for (int i = 0; i < MapManage.Instance.meshxy.x; i++)
        {
            for (int j = 0; j < MapManage.Instance.meshxy.y; j++)
            {
                if (Vector3.Distance(pos, MapManage.Instance.meshpos[i, j]) < min)
                {
                    min = Vector3.Distance(pos, MapManage.Instance.meshpos[i, j]);
                    ans = MapManage.Instance.meshpos[i, j];
                    mesh = new Vector2Int(i, j);
                }
            }
        }
        return ans;
    }

    public void AddPlant(Seed seed)
    {
        OriginCardSeed = seed;
        card = seed.card;
        OriginCardSeed.incool = Cardtype.InHand;
        if (hand != null)
        {
            PoolManage.Instance.PushGameObject(hand.name, hand);
        }
        hand = GetPlantPrefab(card);
        if (hand != null)
        {
            hand = PoolManage.Instance.GetPoolGameObject("Plant", card.Prefab.name);
            hand.GetComponent<SortingGroup>().sortingOrder = 100;
        }
        cellhand.SetActive(true);
        cellhand.GetComponentInChildren<SpriteRenderer>().sprite = Attribute.GetSprite(seed.card.Prefab.name);
        this.oneupdateprotect = true;
    }
    public void FastCellPlant(Vector2Int pos,int id)
    {
        card = Attribute.Instance.GetPlantInfo(id);
        CellPlant(pos, CellPlanttype(pos, id,false),true);
    }
    public void LifeCellPlant(Vector2Int pos, int id,PlantPosType plantPosType,out PlantBase plant)
    {
        card = Attribute.Instance.GetPlantInfo(id);
        plant = SetPlantCell(pos, id);
    }
    /// <summary>
    /// 种植判定
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="plantid">植物id</param>
    /// <returns>判定结果</returns>
    public CellPlantType CellPlanttype(Vector2Int pos,int plantid,bool noramlcreate = true)
    {
        Vector2Int mesh = new Vector2Int();
        if (noramlcreate && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), GetStandWorldPoint(Vector3, ref mesh)) >= 0.7f)
        {
            return CellPlantType.Canot;
        }
        if (plantid == 503)
        {
            return CellPlantType.Present;
        }
        if (MapManage.Instance.CanDye(pos.x,pos.y, Attribute.Instance.GetPlantInfo(plantid).Plantpostype, Attribute.Instance.GetPlantInfo(plantid)))
        {
            return CellPlantType.CanDye;
        }
        if (MapManage.Instance.CanFusion(pos.x, pos.y, Attribute.Instance.GetPlantInfo(plantid).Plantpostype, Attribute.Instance.GetPlantInfo(plantid)))
        {
            return CellPlantType.Fusion; 
        }
        if (MapManage.Instance.CanCell(pos.x, pos.y, Attribute.Instance.GetPlantInfo(plantid).Plantpostype))
        {
            return CellPlantType.Cell;
        }
        return CellPlantType.Canot; 
    }

    /// <summary>
    /// 执行种植操作
    /// </summary>
    /// <param name="pos">坐标</param>
    public void CellPlant(Vector2Int pos, CellPlantType cellPlantType,bool notusesun = false)
    {
        if (!notusesun && cellPlantType != CellPlantType.CanDye)
        {
            BattleManage.Instance.SunnumberChange(-card.Cost);
        }
        switch (cellPlantType)
        {
            case CellPlantType.Present:
                DefaultCellPlant(pos);
                break;
            case CellPlantType.CanDye:
                GoToChangePlant(card.ID, pos);
                break;
            case CellPlantType.Fusion:
                GoToChangePlant(Attribute.Instance.GetFusion(card.ID, MapManage.Instance.meshPlants[pos.x, pos.y].GetPlant(Attribute.Instance.GetPlantInfo(card.ID).Plantpostype).unitInfo.ID), pos);
                break;
            case CellPlantType.Cell:
                DefaultCellPlant(pos);
                break;
        }
    }

    void CardToCool()
    {
        cellhand?.SetActive(false);
        OriginCardSeed?.ToCool();
        OriginCardSeed = null;
    }
    public void GoToChangePlant(int id,Vector2Int pos)
    {
        MapManage.Instance.meshPlants[pos.x, pos.y].GetPlant(Attribute.Instance.GetPlantInfo(card.ID).Plantpostype).Destory();//删除手中和地上的植物
        if (hand != null)
        {
            PoolManage.Instance.PushGameObject(hand.name, hand);
        }
        SetPlantCell(pos, id);
        CreateCellEffect(pos);
    }

    /// <summary>
    /// 正常种植
    /// </summary>
    public void DefaultCellPlant(Vector2Int pos)
    {
        if (hand != null)
        {
            PoolManage.Instance.PushGameObject(hand.name, hand);
        }
        SetPlantCell(pos, card.ID);
        CreateCellEffect(pos);
    }

    /// <summary>
    /// 种植植物
    /// </summary>
    /// <param name="xy">网格坐标</param>
    /// <param name="id">植物id</param>
    public PlantBase SetPlantCell(Vector2Int xy,int id)//种植植物
    {
        PlantBase plantBase = null;
        if (Attribute.Instance.GetPlantInfo(id) == null)
        {
            DebugShow.Instance.Init("试图生成不存在的植物！");
            return null;
        }
        Attribute.PlantInfo cardInfo = Attribute.Instance.GetPlantInfo(id).Clone();//查找卡片信息
        if (hand != null)
        {
            PoolManage.Instance.PushGameObject(hand.name, hand);
        }
        hand = GameObject.Instantiate(GetPlantPrefab(cardInfo), PlantManage.transform);//生成植物预制体
        hand.transform.position = MapManage.Instance.meshpos[xy.x, xy.y];//调整植物坐标
        hand.GetComponentInChildren<Animator>().enabled = true;//激活动画
        if (id != 503)
        {
            hand.GetComponentInChildren<BoxCollider2D>().enabled = true;//激活碰撞箱
        }
        this.ToChangeDreamDepth(id);//更新梦境深度
        plantBase = hand.GetComponentInChildren<PlantBase>();//开始录入植物信息
        plantBase.Init(xy, cardInfo);
        BattleManage.Instance.controlBase?.OnCellPlant(plantBase);//触发事件
        hand = null;//删除手中植物
        return plantBase;
    }
    /// <summary>
    /// 更新梦境深度
    /// </summary>
    /// <param name="id">新增植物id</param>
    public void ToChangeDreamDepth(int id)
    {
        if (!cellplantid.Contains(id))//添加梦境深度
        {
            cellplantid.Add(id);
            BattleManage.Instance.LevelDreamDepth = 0;
            switch (Attribute.Instance.filedInfo.Difficulty)
            {
                case 2:
                    BattleManage.Instance.LevelDreamDepth = 2;
                    break;
                case 3:
                    BattleManage.Instance.LevelDreamDepth = 4;
                    break;
                case 4:
                    BattleManage.Instance.LevelDreamDepth = 6;
                    break;
                case 5:
                    BattleManage.Instance.LevelDreamDepth = 10;
                    break;
            }
            foreach (int cellid in cellplantid)
            {
                BattleManage.Instance.LevelDreamDepth += Attribute.Instance.GetPlantInfo(cellid).DreamAdd;
            }
            BattleManage.Instance.ChangeDreamDepth();
        }
    }

    /// <summary>
    /// 生成种植特效
    /// </summary>
    /// <param name="pos">位置坐标</param>
    public void CreateCellEffect(Vector2Int pos)
    {
        Vector3 pspos = MapManage.Instance.meshpos[pos.x, pos.y];
        pspos.y -= 0.5f;
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "CellPlantPS", pspos);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
        MusicManage.Instance.PlayEffect("plant", 1);
    }


    /// <summary>
    /// 生成植物预制体
    /// </summary>
    /// <param name="card">植物卡片</param>
    /// <returns>生成的植物</returns>
    private GameObject GetPlantPrefab(Attribute.PlantInfo card)
    {
        return card.Prefab;
    }











}
