using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static BuffManage;

public class Plant_FumeShroom : PlantBase
{
    protected class FumeBullet : IEnchantment
    {
        public Action<BattleUnitModel, int> Action { get; set ; }
    }
    protected FumeBullet _FumeBullet;
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        _FumeBullet = new FumeBullet();
    }
    public override void OnAttack()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "BigPuffShroom", ObjCreateTsf.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(gameObject).Init(2f);
        MusicManage.Instance.PlayEffect("fume", 0.5f);
    }
    public void ToGiveDamage()
    {
        RaycastHit2D[] raycastHit2D = BattleManager.GetRayAll(this.transform.position, Vector2.right, 8, 2);
        if (raycastHit2D != null && raycastHit2D.Length > 0)
        {
            _FumeBullet.Action = null;
            foreach (EnchantmentBase enchantment in this.component._enchantment)
            {
                _FumeBullet.Action += enchantment.Enchantemnt;
            }
            foreach(BattleUnitBuf battleUnitBuf in this.bufDetail.GetBufList())
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
                    zombies.TakeDamage(new DamageObject(unitInfo.Damage,Bullettype.PuffShroom, this));
                    _FumeBullet.Action?.Invoke(zombies, this.Damage(unitInfo.Damage, DamageElement.Default));
                    FumeShroomHit(zombies);
                }
            }
        }
    }
    public virtual void FumeShroomHit(ZombiesBase zombie)
    {

    }
    bool RayHit()
    {
        RaycastHit2D raycastHit2D = BattleManager.GetRay(this.transform.position, Vector2.right, 8, 2);
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
        return base.CanAttack() && RayHit();
    }
}
