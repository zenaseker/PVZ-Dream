using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitModel : MonoBehaviour
{
    [HideInInspector] public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    public UnitBufDetail bufDetail;
    public Attribute.UnitInfo unitInfo;
    protected float speed = 1f;

    public int MaxHP
    {
        get
        {
            return unitInfo.HP + bufDetail.MaxHpAdd;
        }
    }
    public int HP;
    public float HPFloat
    {
        get
        {
            return (float)HP / (float)MaxHP;
        }
    }
    public float Speed()
    {
        return speed * bufDetail.SpeedChange();
    }
    public int Damage(int dmg,DamageElement type)
    {
        return (int)(unitInfo.Damage * bufDetail.GiveDamageChange(dmg,type));
    }

    private void FixedUpdate()
    {
        if (BattleManage.Instance.battleStage < BattleStage.BattleReady)
        {
            return;
        }
        OnUpdate();
    }
    protected virtual void OnUpdate()
    {

    }
    public virtual void ChangeSpeed()
    {
        this.GetComponent<Animator>().speed = this.Speed();
    }
    public virtual void ColorChange()
    {
        foreach (SpriteRenderer spriteRenderer in this.spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = bufDetail.GetColor();
            }
        }
    }
    public virtual void TakeDamage(DamageObject damageObject)
    {

    }
    public virtual void ReCoverHp(int hp)
    {
        this.HP += hp;
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ReCoverHpPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(1f);
        this.OnHpChange(hp);
        if (this.HP > this.MaxHP)
        {
            this.HP = this.MaxHP;
        }
    }
    public virtual void Kill(BattleUnitModel target)
    {

    }
    public virtual void OnHpChange(int num)
    {

    }
    public void OnDestroy()
    {
        DOTween.Kill(this.gameObject, true);
    }
    public void MapAreaAction(Vector2Int xy, MapActionType mapActionType)
    {
        switch(mapActionType)
        {
            case MapActionType.RangeCherry:
                AreaActionRangeCherry(xy);
                break;
            case MapActionType.RangeLight:
                AreaActionRangeLight(xy);
                break;
            case MapActionType.RangeSnow:
                AreaActionRangeSnow(xy);
                break;
            case MapActionType.LineFire:
                AreaActionLineFire(xy.x);
                break;
            case MapActionType.ScreenLight:
                AreaActionScreenLight();
                break;
            case MapActionType.ScreenSnow:
                AreaActionScreenSnow();
                break;

        }
    }

    public virtual bool CanAddBuf(BattleUnitBuf buf)
    {
        return true;
    }

    public virtual void AreaActionRangeCherry(Vector2Int xy)
    {

    }
    public virtual void AreaActionRangeLight(Vector2Int xy)
    {

    }
    public virtual void AreaActionRangeSnow(Vector2Int xy)
    {

    }
    public virtual void AreaActionLineFire(int line)
    {

    }
    public virtual void AreaActionScreenLight()
    {

    }
    public virtual void AreaActionScreenSnow()
    {

    }
}
