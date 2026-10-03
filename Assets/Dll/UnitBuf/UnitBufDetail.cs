using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum KeyWordBuf
{
    Default,//ÆÕÍ¨
    Cold,//º®Àä
    MoonErosion,//ÔÂ»ªÇÖÊ´
    MindControl,//÷È»ó
    Possess,//¸½Éí
}

public class UnitBufDetail
{
    public BattleUnitModel owner;
    public List<BattleUnitBuf> buflist = new List<BattleUnitBuf>();
    public List<BattleUnitBuf> removelist = new List<BattleUnitBuf>();

    public UnitBufDetail(BattleUnitModel owner)
    {
        this.owner = owner;
        OnUpdate += UPDATE;
        OnTakeDamage += ElementDamage;
    }
    public List<BattleUnitBuf> GetBufList()
    {
        return buflist;
    }
    public BattleUnitBuf AddBuf(BattleUnitBuf buf, int stack = 0, float countDown = 0f)
    {
        if (!owner.CanAddBuf(buf)) return buf;
        BattleUnitBuf tbuf = buflist.Find(x => x.GetType() == buf.GetType());
        if (tbuf != null)
        {
            tbuf.AddStack(stack, countDown);
            return tbuf;
        }
        buflist.Add(buf); 
        tbuf = buflist.Find(x => x == buf);
        tbuf.Init(this.owner);
        tbuf.AddStack(stack, countDown);
        return tbuf;
    }
    public BattleUnitBuf AddKeyWordBuf(KeyWordBuf keyWordBuf, int stack = 0, float countDown = 0f)
    {
        BattleUnitBuf buf = buflist.Find(x => x.KeyWordBuf == keyWordBuf);
        if (buf == null)
        {
            if (stack <= 0) return null;
            buf = AddBuf(BuffManage.GetKeyWordBuf(keyWordBuf),stack,countDown);
            return buf;
        }
        if (!owner.CanAddBuf(buf)) return buf;
        buf.AddStack(stack,countDown);
        return buf;
    }
    public BattleUnitBuf GetKeyWordBuf(KeyWordBuf keyWordBuf)
    {
        BattleUnitBuf buf = buflist.Find(x => x.KeyWordBuf == keyWordBuf);
        return buf;
    }
    public void RemoveBuf(Predicate<BattleUnitBuf> predicate)
    {
        BattleUnitBuf plantBuf = buflist.Find(predicate);
        if (plantBuf!=null)
        {
            plantBuf.Destory();
        }
    }
    public int MaxHpAdd
    {
        get
        {
            int num = 0;
            foreach(BattleUnitBuf buf in buflist)
            {
                num += buf.MaxHp();
            }
            return num;
        }
    }
    public float ProductTime
    {
        get
        {
            float num = 1;
            foreach (BattleUnitBuf buf in buflist)
            {
                num *= buf.ProductTime();
            }
            foreach (MapMeshUnitBuf buf in MapManage.Instance.meshPlants[((PlantBase)this.owner).XY.x, ((PlantBase)this.owner).XY.y].mapbuflist)
            {
                num *= buf.Product((PlantBase)this.owner);
            }
            return num;
        }
    }

    public float EatingTime
    {
        get
        {
            float num = 0;
            foreach (BattleUnitBuf buf in buflist)
            {
                num += buf.EatingTime();
            }
            return num;
        }
    }
    public float StartSpeedMax
    {
        get
        {
            float num = 0;
            foreach (BattleUnitBuf buf in buflist)
            {
                num += buf.StartMaxSpeed();
            }
            return num;
        }
    }
    public float StartSpeedMin
    {
        get
        {
            float num = 0;
            foreach (BattleUnitBuf buf in buflist)
            {
                num += buf.StartMinSpeed();
            }
            return num;
        }
    }
    public float SpeedChange()
    {
        float num = 1;
        if (this.owner is ZombiesBase && (this.owner as ZombiesBase).isdie)
        {
            return 1;
        }
        foreach (BattleUnitBuf buf in buflist)
        {
            num *= buf.SpeedChange();
        }
        return num;
    }
    public float TakeDamageChange(int dmg, DamageElement damageType)
    {
        float num = 1;
        foreach (BattleUnitBuf buf in buflist)
        {
            num *= buf.TakeDamageChange(dmg,damageType);
        }
        return num;
    }
    public float GiveDamageChange(int dmg, DamageElement damageType)
    {
        float num = 1;
        foreach (BattleUnitBuf buf in buflist)
        {
            num *= buf.GiveDamageChange(dmg, damageType);
        }
        return num;
    }
    public Color GetColor()
    {
        Color color = Color.white;
        foreach (BattleUnitBuf buf in buflist)
        {
            color = Color.Lerp(color, buf.UnitColor(), 0.5f);
        }
        return color;
    }
    public Action OnDie;
    public Action<int, DamageElement> OnTakeDamage;
    public Action<GameObject> OnProduct;
    public Action<float> OnUpdate;
    public Action<DamageObject> OnAttack;
    public Action<IEnchantment> OnGetBullet;
    private void UPDATE(float deltatime)
    {
        if (removelist.Count > 0)
        {
            foreach (BattleUnitBuf battleUnitBuf in removelist)
            {
                buflist.Remove(battleUnitBuf);
                battleUnitBuf.OnDestory();
            }
            removelist.Clear();
        }
    }
    public void OnDestory()
    {
        foreach (BattleUnitBuf battleUnitBuf in buflist)
        {
            battleUnitBuf.OnDestory();
        }
        buflist.Clear();
    }
    private void ElementDamage(int dmg,DamageElement damageType)
    {
        if (damageType == DamageElement.Fire && GetKeyWordBuf(KeyWordBuf.Cold) != null)
        {
            GetKeyWordBuf(KeyWordBuf.Cold).Destory();
        }
    }
}
