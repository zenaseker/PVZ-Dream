using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuffManage;

public class Plant_DarkFrostFumeShroom : Plant_FumeShroom
{

    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "BigPuffShroom_DarkFrost", ObjCreateTsf.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(gameObject).Init(2f);
        MusicManage.Instance.PlayEffect("fume", 0.5f);
    }
    public override void FumeShroomHit(ZombiesBase zombie)
    {
        int count = 1;
        if (zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion) == null)
        {
            return;
        }
        if (zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.Cold) != null)
        {
            count = zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.Cold).stack;
            if (count > zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).stack)
            {
                count = zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).stack;
            }
        }
        zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).AddStack(-count, 0);
        zombie.bufDetail.AddBuf(new BattleUnitBuf_FrostErosion(), count);

    }

    public void GiveDamage()
    {
        RaycastHit2D[] raycastHit2D = BattleManager.GetRayAll(this.transform.position, Vector2.right, 12, 2);
        if (raycastHit2D != null && raycastHit2D.Length > 0)
        {
            _FumeBullet.Action = null;
            foreach (EnchantmentBase enchantment in this.component._enchantment)
            {
                _FumeBullet.Action += enchantment.Enchantemnt;
            }
            foreach (BattleUnitBuf battleUnitBuf in this.bufDetail.GetBufList())
            {
                if (battleUnitBuf is BattleUnitBuf_Possess)
                {
                    BattleUnitBuf_Possess battleUnitBuf_Possess = battleUnitBuf as BattleUnitBuf_Possess;
                    battleUnitBuf_Possess.OnGetBullet(_FumeBullet);
                }
            }
            foreach (RaycastHit2D ray in raycastHit2D)
            {
                if (ray.transform.tag == "Zombie" && !ray.transform.GetComponent<ZombiesBase>().IgnoreSpecialAttack.Contains(IgonrePlant.LineAttacker))
                {
                    ZombiesBase zombies = ray.transform.GetComponent<ZombiesBase>();
                    zombies.TakeDamage(new DamageObject(unitInfo.Damage, Bullettype.PuffShroom, this));
                    _FumeBullet.Action?.Invoke(zombies, this.Damage(unitInfo.Damage, DamageElement.Default));
                    FumeShroomHit(zombies);
                }
            }
        }
    }
    bool rayHit()
    {
        RaycastHit2D raycastHit2D = BattleManager.GetRay(this.transform.position, Vector2.right, 12, 2);
        if (raycastHit2D)
        {
            GameObject ray = raycastHit2D.transform.gameObject;
            if (ray.tag == "Zombie" && !ray.GetComponent<ZombiesBase>().IgnoreSpecialAttack.Contains(IgonrePlant.LineAttacker))
            {
                return true;
            }
        }
        return false;
    }
    protected override bool CanAttack()
    {
        return this._CanAttack && rayHit();
    }
    public class BattleUnitBuf_FrostErosion : BattleUnitBuf
    {
        int effect = 0;
        public override void OnAdd()
        {
            base.OnAdd();
            if (this.stack >= 15 && effect == 0)
            {
                effect++;
                GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "MoonErosionBroom", this._owner.transform.position);
                RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(2);
                this._owner.TakeDamage(new DamageObject((int)(this._owner.MaxHP * 0.3f), Bullettype.Buf, null) { DamageElement = DamageElement.Snow });
            }
            if (this.stack >= 40 && effect <= 1)
            {
                effect++;
                GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", this._owner.transform.position);
                RandomUtil.AddOrGetComponent<FrostStream>(obj).Init(3, 3.0f, 0.5f);
            }
        }
        public override void OnDie()
        {
            if (this.stack >= 65)
            {
                GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "DarkFrostStream", this._owner.transform.position);
                RandomUtil.AddOrGetComponent<DarkFrostStream>(obj).Init(this.stack * 10, this.stack / 20);
            }
            base.OnDie();
        }
    }
}
