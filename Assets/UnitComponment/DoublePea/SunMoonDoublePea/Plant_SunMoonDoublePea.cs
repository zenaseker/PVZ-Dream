using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SunMoonState
{
    None,
    Sun,
    Moon,
}
public class Plant_SunMoonDoublePea : Plant_DoublePea
{
    public SpriteRenderer Sun;
    public SpriteRenderer Moon;
    public int SunMoon = 0;
    public SunMoonState InState = SunMoonState.None;
    public float StateTime = -1f;

    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        if (StateTime > 0f)
        {
            StateTime -= Time.deltaTime;
            if (StateTime <= 0f)
            {
                this.InState = SunMoonState.None;
                this.StateTime = -1f;
                this.SunMoon = 0;
                Sun.color = new Color(Sun.color.r, Sun.color.g, Sun.color.b, 0);
                Moon.color = new Color(Moon.color.r, Moon.color.g, Moon.color.b, 0);
                ChangeLight();
            }
        }
    }
    public override void OnAttack()
    {
        float num = 0.5f;
        num += this.component._matrix._RegetMatrixnum * 0.02f;
        num -= this.component._matrix._RegetMatrixnum2 * 0.02f;
        if (InState == SunMoonState.Sun)//处于光时固定发射光
        {
            InitSunBullet();
        }
        else if (InState == SunMoonState.Moon)//处于暗时固定发射暗
        {
            InitMoonBullet();
        }
        else if(num > Random.Range(0f, 1f))//处于普时概率发射光
        {
            InitSunBullet();
        }
        else
        {
            InitMoonBullet();
        }
        ChangeLight();
    }

    public void InitSunBullet()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "LightPeaBullet2", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), (int)this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Light));
        InitBullet(gameObject.GetComponent<IEnchantment>());
        if (InState == SunMoonState.None)
        {
            this.SunMoon++;
        }
        if (SunMoon >= 10)
        {
            ToState(true);
        }
    }

    public void InitMoonBullet()
    {
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("Bullet", "NightPeaBullet", ObjCreateTsf.position);
        gameObject.GetComponent<BulletBase>().Init(new Vector2(5.5f, 0), (int)this.XY.x, UnitFaction.Plant, this.Damage(unitInfo.Damage, DamageElement.Dark));
        InitBullet(gameObject.GetComponent<IEnchantment>());
        if (InState == SunMoonState.None)
        {
            this.SunMoon--;
        }
        if (SunMoon <= -10)
        {
            ToState(false);
        }
    }
    public void ChangeLight()
    {
        Sun.color = new Color(Sun.color.r, Sun.color.g, Sun.color.b, 0);
        Moon.color = new Color(Moon.color.r, Moon.color.g, Moon.color.b, 0);
        if (SunMoon > 0)
        {
            Sun.color = new Color(Sun.color.r, Sun.color.g, Sun.color.b, (float)(0.15 * SunMoon));
            if (SunMoon >= 10)
            {
                Sun.color = new Color(Sun.color.r, Sun.color.g, Sun.color.b, 1);
            }
        }
        else
        {
            Moon.color = new Color(Moon.color.r, Moon.color.g, Moon.color.b, (float)(0.15 * SunMoon));
            if (SunMoon <= 10)
            {
                Moon.color = new Color(Moon.color.r, Moon.color.g, Moon.color.b, 1);
            }
        }
    }

    public void ToState(bool sun)
    {
        if (this.InState == SunMoonState.None) return;
        if (sun)
        {
            this.InState = SunMoonState.Sun;
        }
        else
        {
            this.InState = SunMoonState.Moon;
        }
        this.StateTime = 10f;
    }
}
