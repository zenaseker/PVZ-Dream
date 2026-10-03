using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class Present : PlantBase
{
    static Dictionary<int, List<int>> RandomID = null;//0：原版；1：梦境；2：梦源；3：梦元素
    static bool gn = false;
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        if (RandomID == null)
        {
            GetRandomId();
        }
        unitInfo = plant;
        XY = xy;
        this.GetComponent<SortingGroup>().sortingOrder = 1000;
        inhand = false;
    }
    void GetRandomId()
    {
        RandomID = new Dictionary<int, List<int>>();
        foreach (Attribute.PlantInfo plantInfo in PlantInfos.PlantInfoInGame.Values)
        {
            if (plantInfo.ID == 503 || plantInfo.ID == 11)
            {
                continue; 
            }
            if (plantInfo.ID / 100 <= 0)
            {
                if (!RandomID.ContainsKey(0))
                {
                    RandomID.Add(0, new List<int>());
                }
                RandomID[0].Add(plantInfo.ID);
            }
            else if (plantInfo.ID / 100 > 0 && plantInfo.ID / 100 < 10)
            {
                if (!RandomID.ContainsKey(1))
                {
                    RandomID.Add(1, new List<int>());
                }
                if (plantInfo.DreamElement.Contains(Attribute.DreamElement.Dream))
                {
                    if (!RandomID.ContainsKey(3))
                    {
                        RandomID.Add(3, new List<int>());
                    }
                    RandomID[3].Add(plantInfo.ID);
                    continue;
                }
                RandomID[1].Add(plantInfo.ID);
            }
            else
            {
                if (!RandomID.ContainsKey(2))
                {
                    RandomID.Add(2, new List<int>());
                }
                RandomID[2].Add(plantInfo.ID);
            }
        }
    }
    protected override void OnUpdate()
    {
        return;
    }
    public void AnEnd()
    {
        DOTween.Kill(this.gameObject, true);
        GameObject.Destroy(this.gameObject);
    }
    public void PresentOpen()
    {
        List<PlantBase> plantBases = MapManage.Instance.meshPlants[this.XY.x, this.XY.y].GetPlants();
        Debug.Log(string.Join(",", plantBases));
        if (plantBases.Count <= 0)
        {
            NoPlant();
        }
        else
        {
            if (PlantDye(plantBases))
            {
                return;
            }
            NotAllPlant(plantBases);
        }
    }
    void NoPlant()
    {
        int ramdonid = 0;
        float ro = Random.Range(0f, 1f);
        if (ro <= 0.55f)
        {
            ramdonid = 0;
        }
        else if (ro <= 0.85f)
        {
            ramdonid = 1;
        }
        else if (ro < 0.99f)
        {
            ramdonid = 2;
        }
        else
        {
            ramdonid = 3;
        }
        ramdonid = RandomUtil.SelectOne(RandomID[ramdonid]);
        if (!gn && Random.Range(0f,1f) < 0.5f)
        {
            gn = true;
            ramdonid = 502;
        }
        HandManage.Instance.FastCellPlant(this.XY, ramdonid);
    }
    void NotAllPlant(List<PlantBase> plantBases)
    {
        List<PlantPosType> plantPosTypes = new List<PlantPosType> { PlantPosType.Default, PlantPosType.Base, PlantPosType.Shell, PlantPosType.Top, PlantPosType.Little };

        foreach (PlantBase plantBase in plantBases)
        {
            if (plantPosTypes.Contains((plantBase.unitInfo as Attribute.PlantInfo).Plantpostype))
            {
                plantPosTypes.Remove((plantBase.unitInfo as Attribute.PlantInfo).Plantpostype);
            }
        }
        List<Attribute.PlantInfo> ids = Attribute.PlantInfos.PlantInfoes.FindAll(x => plantPosTypes.Contains(x.Plantpostype));
        if (ids.Count <= 0)
        {
            return;
        }
        int ramdonid = 0;
        float ro = Random.Range(0f, 1f);
        if (ro <= 0.45f)//0.55
        {
            ramdonid = 0;
            ids.RemoveAll(x => x.ID / 100 > 0);
        }
        else if (ro <= 0.65f)//0.85
        {
            ramdonid = 1;
            ids.RemoveAll(x => x.ID / 100 <= 0 || x.ID / 100 > 10);
        }
        else if (ro < 0.8f)//0.99
        {
            ramdonid = 2;
            ids.RemoveAll(x => x.ID / 100 < 10);
        }
        else
        {
            ramdonid = 3;
            ids.RemoveAll(x => x.ID / 100 != 5);
        }
        if (ids.Count <= 0)
        {
            return;
        }
        ramdonid = RandomUtil.SelectOne(ids).ID;

        HandManage.Instance.FastCellPlant(this.XY, ramdonid);
    }
    bool PlantDye(List<PlantBase> plantBases)
    {
        int i = -1;
        while (i < plantBases.Count - 1)
        {
            i++;
            int orid = plantBases[i].unitInfo.ID;
            if (orid % 100 == 99)
            {
                continue;
            }
            if (orid / 100 == 0)
            {
                List<int> ids = RandomID[1].FindAll(x => x % 100 == orid % 100);
                if (ids.Count <= 0)
                {
                    continue;
                }
                int endid = RandomUtil.SelectOne(ids);
                PresentCellPlant(this.XY, endid, (plantBases[i].unitInfo as Attribute.PlantInfo).Plantpostype);
                return true;
            }
            else if (orid / 100 > 0 && orid / 100 < 10)
            {
                if (orid / 100 == 5)
                {
                    continue;
                }
                List<int> ids = RandomID[2].FindAll(x => x % 100 == orid % 100);
                if (ids.Count <= 0)
                {
                    continue;
                }
                int endid = RandomUtil.SelectOne(ids);
                PresentCellPlant(this.XY, endid, (plantBases[i].unitInfo as Attribute.PlantInfo).Plantpostype);
                return true;
            }
        }
        return false;
    }
    public void PresentCellPlant(Vector2Int pos, int id, PlantPosType plantPosType)
    {
        MapManage.Instance.meshPlants[pos.x, pos.y].GetPlant(plantPosType)?.Destory();//删除手中和地上的植物
        HandManage.Instance.SetPlantCell(pos, id);
        HandManage.Instance.CreateCellEffect(pos);
    }
}
