using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BuffManage
{
    private static BuffManage __instance;

    public static BuffManage Instance
    {
        get
        {
            if (__instance == null)
            {
                __instance = new BuffManage();
            }
            return __instance;
        }
    }

    public static BattleUnitBuf GetKeyWordBuf(KeyWordBuf keyWordBuf)
    {
        switch (keyWordBuf)
        {
            case KeyWordBuf.Cold:
                return new BattleUnitBuf_Snow();
            case KeyWordBuf.MoonErosion:
                return new BattleUnitBuf_MoonErosion();
            case KeyWordBuf.MindControl:
                return new BattleUnitBuf_Hypnos();
        }
        return null;
    }
    public List<BattleUnitBuf> GetZombieDreamDepthIncrease()
    {
        List<BattleUnitBuf> BattleUnitBufs = new List<BattleUnitBuf>();
        if (BattleManage.Instance.LevelDreamDepth > 0)
        {
            BattleUnitBufs.Add(new ZombieDreamBufList.ZombieBuf_MaxHpUp { stack = BattleManage.Instance.LevelDreamDepth + Attribute.Instance.filedInfo.Difficulty * 10 });
        }
        if (BattleManage.Instance.LevelDreamDepth > 2)
        {
            BattleUnitBufs.Add(new ZombieDreamBufList.ZombieBuf_StartSpeedUp { stack = BattleManage.Instance.LevelDreamDepth / 2 });
        }

        return BattleUnitBufs;
    }

    public List<BattleUnitBuf> GetPlantDreamDepthIncrease()
    {
        List<BattleUnitBuf> BattleUnitBufs = new List<BattleUnitBuf>();
        return BattleUnitBufs;
    }

    public class ZombieDreamBufList
    {
        public class ZombieBuf_MaxHpUp : BattleUnitBuf
        {
            public override int MaxHp()
            {
                return (int)(_owner.unitInfo.HP  * stack * 0.02f);
            }
        }
        public class ZombieBuf_StartSpeedUp : BattleUnitBuf
        {
            float min = 0f;
            float max = 0f;
            public override void OnInit()
            {
                if (stack <= 4)
                {
                    max = 0.1f * stack;
                }
                else if (stack <= 10)
                {
                    max = 0.4f;
                    min = 0.1f * (stack - 4);
                }
                else
                {
                    max = 0.4f;
                    min = 0.6f;
                }
            }
            public override float StartMaxSpeed()
            {
                return max;
            }
            public override float StartMinSpeed()
            {
                return min;
            }
        }
    }
    public class PlantDreamBufList
    {

    }
    public class BattleUnitBuf_AttackUp : BattleUnitBuf
    {
        public override void OnUpdate(float deltatime)
        {
            countdown -= deltatime;
            if (countdown <= 0)
            {
                this.Destory();
            }
        }
        public override float GiveDamageChange(int dmg, DamageElement damageType)
        {
            return this.stack * 0.1f + 1;
        }
    }
    public class BattleUnitBuf_DamageDown : BattleUnitBuf
    {
        public override void OnUpdate(float deltatime)
        {
            countdown -= deltatime;
            if (countdown <= 0)
            {
                this.Destory();
            }
        }
        public override float TakeDamageChange(int dmg, DamageElement damageType)
        {
            return 1 - this.stack * 0.1f;
        }
        public override float GiveDamageChange(int dmg, DamageElement damageType)
        {
            return this.stack * 0.1f + 1;
        }
    }
    public class BattleUnitBuf_Snow : BattleUnitBuf
    {
        GameObject IceTrap = null;
        bool InTrop = false;
        public override KeyWordBuf KeyWordBuf
        {
            get
            {
                return KeyWordBuf.Cold;
            }
        }
        private int beforestack = 0;
        public override void OnInit()
        {
            base.OnInit();
            this.Maxstack = 4;
        }
        public override void OnUpdate(float deltatime)
        {
            countdown -= deltatime;
            if (countdown <= 0)
            {
                this.Destory();
            }
        }
        public override Color UnitColor()
        {
            return Color.blue;
        }
        public override void AddStack(int stack, float countDown)
        {
            if (stack > beforestack)
            {
                this.stack = beforestack = stack;
            }
            this._owner.ColorChange();
            this._owner.ChangeSpeed();
            if (this.stack >= 4)
            {
                if (!InTrop)
                {
                    this.countdown = 5f;
                    IceTrap = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "IceTrap", this._owner.transform);
                    IceTrap.transform.localPosition = new Vector3(0, 0, 0);
                    InTrop = true;
                }
                return;
            }
            if (this.countdown + countDown > 25 - this.stack * 5)
            {
                this.countdown = 25 - this.stack * 5;
                return;
            }
            this.countdown += countDown;
        }
        public override float SpeedChange()
        {
            return 1 - 0.25f * this.stack;
        }
        public override void OnDestory()
        {
            base.OnDestory();
            this._owner.ColorChange();
            this._owner.ChangeSpeed();
            if (this.IceTrap != null)
            {
                IceTrap.transform.SetParent(null);
                PoolManage.Instance.PushGameObject(IceTrap.name, this.IceTrap);
                GameObject effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "IceTrapDestory", this._owner.transform.position);
                RandomUtil.AddOrGetComponent<TimeDestory>(effect).Init(1f);
            }
        }
    }

    public class BattleUnitBuf_MoonErosion : BattleUnitBuf
    {
        public override KeyWordBuf KeyWordBuf
        {
            get
            {
                return KeyWordBuf.MoonErosion;
            }
        }
        public GameObject effect = null;
        public override void OnInit()
        {
            base.OnInit();
            this.Maxstack = 10;
            effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "MoonErosionBufEffect", this._owner.transform);
            effect.transform.localPosition = Vector3.zero;
        }
        public override void AddStack(int stack, float countDown)
        {
            base.AddStack(stack, countDown);
            if (this.stack <= 0)
            {
                this.Destory();
            }
            if (this._owner is Plant_LunarEclipseScaredyShroom)
            {
                this._owner.ChangeSpeed();
            }
            if (this.stack >= Maxstack)
            {
                MoonBreak();
            }
        }

        public override float SpeedChange()
        {
            if (this._owner is Plant_LunarEclipseScaredyShroom)
            {
                return 1f + this.stack * 0.05f;
            }
            return base.SpeedChange();
        }

        public void MoonBreak()
        {
            MusicManage.Instance.PlayEffect("MoonErosion", 0.5f);
            this._owner.TakeDamage(new DamageObject((int)((this._owner.MaxHP > 4000 ? 4000 : this._owner.MaxHP) * 0.1f * this.Maxstack + 200), Bullettype.None, null));
            GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "MoonErosionBroom", this._owner.transform.position);
            RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(2);
            if (this._owner is Zombie_ChargeFootball)
            {
                ((Zombie_ChargeFootball)this._owner).OnMoonBreak();
            }
            this.Destory();
        }
        public override void OnDie()
        {
            if (effect != null)
            {
                effect.transform.SetParent(null);
                PoolManage.Instance.PushGameObject(effect.name, effect);
            }
            base.OnDie();
        }
        public override void OnDestory()
        {
            if (effect != null)
            {
                effect.transform.SetParent(null);
                PoolManage.Instance.PushGameObject(effect.name, effect);
            }
            base.OnDestory();

        }
    }
    public class BattleUnitBuf_Possess : BattleUnitBuf
    {
        public override KeyWordBuf KeyWordBuf
        {
            get
            {
                return KeyWordBuf.Possess;
            }
        }
        public enum PossessType
        {
            Null,
            PuffShroom,
            FumeShroom,
            Gravebuster,
            Potato,
            IceShroom,
            Chomper,
            Jackson
        }
        public GameObject effect;
        public virtual PossessType possessType
        {
            get
            {
                return PossessType.Null;
            }
        }
        public int WakeUpNum = 0;
        public virtual int BrokenNum
        {
            get
            {
                return 1;
            }
        }
        public override void OnInit()
        {
            base.OnInit(); 
            effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Possess_" + possessType.ToString(), this._owner.transform);
            effect.transform.localPosition = Vector3.zero;
        }
        public virtual void Hit(BattleUnitModel battleUnitModel, int dmg)
        {

        }
        public override void OnDestory()
        {
            base.OnDestory();
            PoolManage.Instance.PushGameObject(effect.name, effect,true);
        }
    }
    public class BattleUnitBuf_PlantSleep : BattleUnitBuf
    {
        public GameObject sleep;
        public override void OnInit()
        {
            base.OnInit();
            sleep = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "zzz", this._owner.transform.position + Vector3.one / 2, this._owner.transform);
        }
        public override void OnDestory()
        {
            base.OnDestory();
            PoolManage.Instance.PushGameObject(sleep.name, sleep,true);
        }
    }
    public class BattleUnitBuf_Hypnos : BattleUnitBuf
    {
        public override KeyWordBuf KeyWordBuf
        {
            get
            {
                return KeyWordBuf.MindControl;
            }
        }
        public override void OnInit()
        {
            base.OnInit();
            ZombiesBase owner = this._owner as ZombiesBase;
            if (!owner.Reverse)
            {
                owner.Reverse = true;
                this._owner.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                this._owner.gameObject.layer = 3;
                this._owner.ColorChange();
                owner.OnMindControl();
                GameObject effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "MindControl", this._owner.transform.position + Vector3.up);
                RandomUtil.AddOrGetComponent<TimeDestory>(effect).Init(1f);
            }
        }
        public override Color UnitColor()
        {
            return new Color32(255, 0, 255, 255);
        }
    }
}
