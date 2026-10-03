using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_ChaosMageApprentice : Zombie_Common
{
    public override void Init(int order, Attribute.ZombieInfo zombieCard, int line, float startspeed = 1)
    {
        base.Init(order, zombieCard, line, startspeed);
        this.GetComponent<Animator>().SetTrigger("Change");
    }

    public override void StartDie()
    {
        base.StartDie();
        this.GetComponent<Animator>().SetTrigger("Change");
    }

    public void Change()
    {
        GameObject plantmag = GameObject.Find("PlantManage");
        if (plantmag.transform.childCount <= 0) return;
        PlantBase plant1 = plantmag.transform.GetChild(Random.Range(0, plantmag.transform.childCount)).gameObject.GetComponent<PlantBase>();
        PlantPosType plantPosType = plant1.posType;
        List<PlantBase> plants = MapManage.Instance.GetAllPlantByPosType(plantPosType);
        plants.Remove(plant1);
        if (plants.Count <= 0) return;
        PlantBase plant2 = RandomUtil.SelectOne(MapManage.Instance.GetAllPlantByPosType(plantPosType));
        if (plant2 == null) return;
        if (plant1 != null && plant2 != null)
        {
            Vector2Int p2pos = plant2.XY;
            MapManage.Instance.DestoryCellPlant(plant1.XY.x, plant1.XY.y, plantPosType);
            MapManage.Instance.DestoryCellPlant(plant2.XY.x, plant2.XY.y, plantPosType);
            plant2.ChangeMeshWithOutCheckPos(plant1.XY);
            plant1.ChangeMeshWithOutCheckPos(p2pos);
        }
    }
    public override void LoseArm()
    {
        base.LoseArm();
        transform.GetChild(0).Find("×ó¼ç").GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).Find("×ó¼ç").GetChild(1).gameObject.SetActive(false);
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieCommonArm", transform.GetChild(0).Find("×ó¼ç").GetChild(0).position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.6f, 0.6f);
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform game = transform.GetChild(0).Find("Í·");
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieChaosMageApprenticeHead", game.position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Í·").gameObject.SetActive(false);
    }
}
