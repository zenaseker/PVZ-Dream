using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Attribute;

/// <summary>
/// 植物位置种类
/// </summary>
public enum PlantPosType
{
    All,//全部
    Default,//普通
    Base,//底座
    Shell,//外壳
    Top,//顶部
    Little//迷你
}

/// <summary>
/// 植物种类
/// </summary>
public enum Plantstics
{
    Shooter,//豌豆
    SunFlower,//向日葵
    Cherry,//樱桃
    Nut,//坚果
    Potato,//土豆
    Chomper,//大嘴花
    Shroom,//蘑菇
    Gravebuster,//墓碑吞噬者
    Cattail,//猫尾草
    Other,//其他
}

public enum PlantType
{
    Production,//生产类
    Attack,//攻击类
    Ashes,//灰烬类
    Fuction,//功能类
    Defensive,//防御类
}//植物类型

public enum PlantState
{
    Default,//待机
    Attack,//攻击
    Building,//成长
    Eating,//咀嚼
    Swallow,//吞咽
    Broom,//爆炸
}

public class PlantBase : BattleUnitModel
{
    public Vector2Int XY;//网格坐标
    public PlantPosType posType;//坐标中位置
    public float atkarea;//攻击范围
    public bool inhand = true;//在手中
    protected float DefaultProductTime = -1f;//生产速度
    public float ProductTime = -1f;//生产时间
    public float Attackinterval = -1f;//攻击间隔
    protected float attackinterval = 0;//攻击间隔
    protected float Productingtime = 0f;//生产倒计时
    public float BuildTime = 0f;//成长倒计时
    public bool isBuild = true;//成长
    public bool InSleep = false;//在睡觉
    public bool _CanAttack = true;//可以攻击
    public bool IgnoreUnit = false;//被僵尸忽略
    public ComponentDetail component = new ComponentDetail();//特性组
    public List<string> ComponentList = new List<string>();//特性列表
    public float HighLighttime = 0f;
    public Transform ObjCreateTsf;
    public void ReInit()
    {
        CancelInvoke();
    }

    public virtual void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        unitInfo = plant;
        bufDetail = new UnitBufDetail(this);
        component = new ComponentDetail();
        if (this.StartBuf() != null)
        {
            bufDetail.AddBuf(this.StartBuf());
        }
        HP = MaxHP;
        XY = xy;
        this.GetComponent<SortingGroup>().sortingOrder = (int)(MapManage.Instance.meshxy.x - xy.x) * 10 + (int)(MapManage.Instance.meshxy.y - xy.y);
        MapManage.Instance.CheckPlantCellPos((int)xy.x,(int)xy.y,this, ((Attribute.PlantInfo)unitInfo).Plantpostype,out posType);
        InitComponent(component, plant);
        component?.Init(this);
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.gameObject.name != "shadow")
            {
                spriteRenderers.Add(spriteRenderer);
            }
        }
        if (((Attribute.PlantInfo)unitInfo).Plantstic == Plantstics.Shroom)
        {
            if (MapManage.Instance.meshPlants[xy.x, xy.y].GetMapMeshUnitBufs(PlantPosType.All).Find(x => x is DefaultMapBuff.MapMeshUnitBuf_Night) == null)
            {
                this.bufDetail.AddBuf(new BuffManage.BattleUnitBuf_PlantSleep(), 1); 
                try
                {
                    this.GetComponent<Animator>().SetBool("Sleep", true);
                }
                catch
                {
                    Debug.Log("Shroom not have Sleep bool");
                }
                this._CanAttack = false;
                this.InSleep = true;
            }
        }
        inhand = false;
    }

    public virtual BattleUnitBuf StartBuf()
    {
        return null;
    }
    public void InitComponent(ComponentDetail componentDetail, PlantInfo plant)
    {
        if (ComponentList == null || ComponentList.Count <= 0)
        {
            return;
        }
        foreach (string _component in ComponentList)
        {
            if (_component.Contains("Aura") && Attribute.Instance.ComponentList.ContainsKey(_component))
            {
                AuraBase component = (AuraBase)Attribute.Instance.ComponentList[_component].Clone();
                componentDetail._aura.Add(component);
            }
            if (_component.Contains("DeathRattle") && Attribute.Instance.ComponentList.ContainsKey(_component))
            {
                DeathRattleBase component = (DeathRattleBase)Attribute.Instance.ComponentList[_component].Clone();
                componentDetail._deathrattle.Add(component);
            }
            if (_component.Contains("Enchantment") && Attribute.Instance.ComponentList.ContainsKey(_component))
            {
                EnchantmentBase component = (EnchantmentBase)Attribute.Instance.ComponentList[_component].Clone();
                componentDetail._enchantment.Add(component);
            }
            if (_component.Contains("Innate") && Attribute.Instance.ComponentList.ContainsKey(_component))
            {
                InnateBase component = (InnateBase)Attribute.Instance.ComponentList[_component].Clone();
                componentDetail._innate.Add(component);
            }
            if (_component.Contains("Matrix") && Attribute.Instance.ComponentList.ContainsKey(_component))
            {
                MatrixBase component = (MatrixBase)Attribute.Instance.ComponentList[_component].Clone();
                componentDetail._matrix = component;
            }
        }
    }
    public void ChangeMesh(Vector2Int mesh)
    {
        MapManage.Instance.DestoryCellPlant(XY.x, XY.y,this.posType);
        foreach (AuraBase _component in component._aura)
        {
            _component.Destory();
        }
        component._matrix?.Destory();
        this.XY = mesh;
        this.GetComponent<SortingGroup>().sortingOrder = (MapManage.Instance.meshxy.x - XY.x) * 10 + (MapManage.Instance.meshxy.y - XY.y);
        this.transform.position = MapManage.Instance.meshpos[XY.x, XY.y];
        MapManage.Instance.CheckPlantCellPos(XY.x, XY.y, this, ((Attribute.PlantInfo)unitInfo).Plantpostype,out posType);
        foreach (AuraBase _component in component._aura)
        {
            _component.Init(component);
        }
        component._matrix?.Init(component);
        this.OnChangeMesh();
    }
    public void ChangeMeshWithOutCheckPos(Vector2Int mesh)
    {
        foreach (AuraBase _component in component._aura)
        {
            _component.Destory();
        }
        component._matrix?.Destory();
        this.XY = mesh;
        this.GetComponent<SortingGroup>().sortingOrder = (MapManage.Instance.meshxy.x - XY.x) * 10 + (MapManage.Instance.meshxy.y - XY.y);
        this.transform.position = MapManage.Instance.meshpos[XY.x, XY.y];
        MapManage.Instance.CellPlant(XY.x, XY.y, this, posType);
        foreach (AuraBase _component in component._aura)
        {
            _component.Init(component);
        }
        component._matrix?.Init(component);
        this.OnChangeMesh();
    }
    public virtual void OnChangeMesh()
    {

    }

    public virtual void OnAttack()
    {
    }
    public void Attack()
    {
        if (!CanAttack()) return;
        this.OnAttack();
        bufDetail.OnAttack?.Invoke(null);
    }
    public void ToAttack()
    {
        this.GetComponent<Animator>()?.SetBool("Attack", CanAttack());
    }
    protected virtual bool CanAttack()
    {
        return this._CanAttack;
    }
    public void InitBullet(IEnchantment enchantment)
    {
        bufDetail.OnGetBullet?.Invoke(enchantment);
        component?.Attack(enchantment);
    }

    public virtual void Product()
    {
    }
    public void OnProduct(GameObject sun)
    {
        bufDetail.OnProduct?.Invoke(sun);
    }
    public float GetProductTime()
    {
        return ProductTime = DefaultProductTime * bufDetail.ProductTime;
    }
    public virtual void Building()
    {

    }
    protected override void OnUpdate()
    {
        if (inhand || InSleep) return;
        OnPlantUpdate();
        bufDetail.OnUpdate?.Invoke(Time.deltaTime);
        if (ProductTime > 0f)
        {
            Productingtime += Time.deltaTime;
            if(Productingtime > ProductTime)
            {
                Product();
                Productingtime = 0f;
            }
        }
        if (Attackinterval > 0f)
        {
            attackinterval -= Time.deltaTime * bufDetail.SpeedChange();
            if (attackinterval <= 0f)
            {
                ToAttack();
                attackinterval = Attackinterval;
            }
        }
        if (!isBuild)
        {
            BuildTime -= Time.deltaTime;
            if (BuildTime < 0f)
            {
                Building();
                BuildTime = -1;
                isBuild = true;
            }
        }
        if (HighLighttime > 0f)
        {
            HighLighttime -= Time.deltaTime;
            if (HighLighttime < 0f)
            {
                ChangeLight(1f);
            }
        }
    }
    protected virtual void OnPlantUpdate()
    {

    }
    public override void TakeDamage(DamageObject damageObject)
    {
        if (MapManage.Instance.meshPlants[this.XY.x, this.XY.y].OnTakeDamage(damageObject)) return;
        if (!damageObject.ToFly && this.posType == PlantPosType.Top) return;
        OnTakeDamage(damageObject.Damage, damageObject.OriginUnit);
        this.HP -= (int)(damageObject.Damage * this.bufDetail.TakeDamageChange(damageObject.Damage, damageObject.DamageElement));
        bufDetail.OnTakeDamage?.Invoke(damageObject.Damage, damageObject.DamageElement);
        this.OnHpChange(damageObject.Damage);
        if (this.HP <= 0)
        {
            this.Die();
            damageObject.OriginUnit?.Kill(this);
            return;
        }
        ChangeLight(1.3f);
        HighLighttime = 0.1f;
    }
    public void ChangeLight(float power, float duartion = 0.1f)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.material.DOFloat(power, "_HighLight", 0.1f);
            }
        }
    }
    public virtual void OnTakeDamage(int dmg,BattleUnitModel attacker,DamageElement damageType = DamageElement.Default)
    {

    }
    public virtual void Die()
    {
        component.Destory();
        bufDetail.OnDie?.Invoke();
        bufDetail.OnDestory();
        Destory();
    }
    public void OnDisable()
    {
        
    }
    public override bool CanAddBuf(BattleUnitBuf buf)
    {
        if (buf.KeyWordBuf == KeyWordBuf.Cold)
        {
            if (unitInfo.ID == 5 || unitInfo.ID == 14 || unitInfo.DreamElement.Contains(DreamElement.Ice))
            {
                return false;
            }
        }
        return base.CanAddBuf(buf);
    }
    public virtual void Destory()
    {
        DOTween.Kill(this.gameObject,true);
        MapManage.Instance.DestoryCellPlant(XY.x, XY.y, posType);
        GameObject.Destroy(this.gameObject);
    }
    public virtual void OnZombieContact(ZombiesBase zombiesBase)
    {

    }
}
