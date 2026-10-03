using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using static Attribute;

public class Glove : PropBase
{
    public PlantBase cloveplant = null;
    Vector3 OriginPos;
    Vector2Int mesh;
    public override void GetRayOnPlant(RaycastHit2D raycastHit)
    {
        if (cloveplant != null) return;
        base.GetRayOnPlant(raycastHit);
    }
    public override void Update()
    {
        base.Update();
        if (cloveplant != null)
        {
            cloveplant.transform.position = this.transform.position;
            HandManage.Instance.cellhand.transform.position = HandManage.Instance.GetStandWorldPoint(this.transform.position,ref mesh);
            return;
        }
    }
    public override void OnClick()
    {
        base.OnClick();
        if (cloveplant == null)
        {
            if (inplant == null)
            {
                return;
            }
            cloveplant = inplant;
            cloveplant.gameObject.layer = 6;
            cloveplant.GetComponent<SortingGroup>().sortingLayerName = "Effcet";
            cloveplant.inhand = true;
            OriginPos = cloveplant.transform.position;
            cloveplant.GetComponent<Animator>().enabled = false;
            cloveplant.GetComponent<BoxCollider2D>().enabled = false;
            HandManage.Instance.cellhand.SetActive(true);
            HandManage.Instance.cellhand.GetComponentInChildren<SpriteRenderer>().sprite =Attribute.GetSprite(cloveplant.unitInfo.Prefab.name);
        }
        else if (MapManage.Instance.CanFusion(mesh.x, mesh.y, cloveplant.posType, ((Attribute.PlantInfo)cloveplant.unitInfo)))
        {
            int id = Attribute.Instance.GetFusion(cloveplant.unitInfo.ID, MapManage.Instance.meshPlants[mesh.x, mesh.y].GetPlant(cloveplant.posType).unitInfo.ID);
            MapManage.Instance.meshPlants[mesh.x, mesh.y].GetPlant(cloveplant.posType).Destory();
            cloveplant.Destory();
            HandManage.Instance.SetPlantCell(mesh, id);
            Vector3 pspos = MapManage.Instance.meshpos[mesh.x,mesh.y];
            pspos.y -= 0.5f;
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "CellPlantPS", pspos);
            RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
            cloveplant = null;
            PropManage.Instance.OnPropUsed();
        }
        else if (MapManage.Instance.CanCell(mesh.x, mesh.y, ((Attribute.PlantInfo)cloveplant.unitInfo).Plantpostype))
        {
            cloveplant.ChangeSpeed();
            cloveplant.ChangeMesh(mesh);
            cloveplant.gameObject.layer = 3;
            cloveplant.GetComponent<SortingGroup>().sortingLayerName = "Plant";
            cloveplant.inhand = false;
            cloveplant.GetComponent<Animator>().enabled = true;
            cloveplant.GetComponent<BoxCollider2D>().enabled = true;
            cloveplant = null;
            HandManage.Instance.cellhand.SetActive(false);
            PropManage.Instance.OnPropUsed();
            MusicManage.Instance.PlayEffect("plant", 1);
        }
    }
    public override void OnHide()
    {
        base.OnHide();
        inplant = null;
        if (cloveplant != null)
        {
            cloveplant.ChangeSpeed();
            cloveplant.inhand = false;
            HandManage.Instance.cellhand.SetActive(false);
            cloveplant.transform.position = OriginPos;
            cloveplant.gameObject.layer = 3;
            cloveplant.GetComponent<SortingGroup>().sortingLayerName = "Plant";
            cloveplant.GetComponent<Animator>().enabled = true;
            cloveplant.GetComponent<BoxCollider2D>().enabled = true;
            HandManage.Instance.cellhand.SetActive(false);
            cloveplant = null;
        }
    }
}
