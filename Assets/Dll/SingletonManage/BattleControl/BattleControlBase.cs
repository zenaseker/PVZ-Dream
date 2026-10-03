using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleControlBase
{
    public GameObject Map = null;
    /// <summary>
    /// 游戏加载
    /// </summary>
    public virtual void OnLevelInit()
    {

    }
    /// <summary>
    /// 选卡前
    /// </summary>
    public virtual void BeforeSelcetCard()
    {

    }
    /// <summary>
    /// 开启战斗
    /// </summary>
    public virtual void OnLevelStart()
    {

    }
    /// <summary>
    /// 开始生成僵尸
    /// </summary>
    public virtual void OnStartCreateZombie()
    {

    }
    /// <summary>
    /// 战中
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
    /// 结束战斗
    /// </summary>
    public virtual void OnLevelEnd()
    {

    }
    /// <summary>
    /// 选择植物卡片
    /// </summary>
    /// <param name="seed"></param>
    public virtual void OnSelcetSeedCard(Seed seed)
    {

    }
    /// <summary>
    /// 选择元素卡片
    /// </summary>
    /// <param name="seed"></param>
    public virtual void OnSelcetSeedCard(ElementSeed seed)
    {

    }
    /// <summary>
    /// 种植植物
    /// </summary>
    /// <param name="plant"></param>
    public virtual void OnCellPlant(PlantBase plant)
    {

    }
    /// <summary>
    /// 切换元素
    /// </summary>
    /// <param name="dreamElement"></param>
    public virtual void OnChangeElement(Attribute.DreamElement dreamElement)
    {

    }
    /// <summary>
    /// 生成僵尸
    /// </summary>
    /// <param name="zombie"></param>
    public virtual void OnCreateZombie(ZombiesBase zombie)
    {

    }
    /// <summary>
    /// 修改生成的僵尸
    /// </summary>
    /// <param name="OrginId"></param>
    /// <returns></returns>
    public virtual int ChangeCreateZombie(int OrginId)
    {
        return OrginId;
    }
}
