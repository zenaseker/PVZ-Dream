using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GNCattail_bullet : BulletBase
{
    public override void OnHit(BattleUnitModel unit)
    {
        base.OnHit(unit);
        unit.bufDetail.AddBuf(new BattleUnitBuf_GNCattail(), 1);
    }
    public override void HitEffect()
    {
        base.HitEffect();
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "GNCattailBulletHitPS", this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(obj).Init(0.5f);
    }
    public override void AfterHit()
    {
        PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
    }
    public class BattleUnitBuf_GNCattail : BattleUnitBuf
    {
        float dingshen = 0f;
        public override void OnInit()
        {
            base.OnInit();
            this.Maxstack = 3;
        }
        public override void OnUpdate(float deltatime)
        {
            base.OnUpdate(deltatime);
            if (dingshen >= 0f)
            {
                dingshen -= Time.deltaTime;
                if (dingshen < 0)
                {
                    this._owner.ChangeSpeed();
                }
            }
        }
        public override void OnAdd()
        {
            base.OnAdd();
            switch (this.stack)
            {
                case 1:
                    dingshen = 0.2f;
                    Jianshe(20);
                    break;
                case 2:
                    dingshen = 0.5f;
                    Jianshe(20);
                    break;
                case 3:
                    this.stack = 0;
                    dingshen = 1f;
                    Jianshe(30);
                    CreateMao();
                    break;
            }
            this._owner.ChangeSpeed();
        }
        void Jianshe(int dmg)
        {
            foreach (var monster in Physics2D.OverlapCircleAll(this._owner.transform.position, 1f, 2))
            {
                if (monster.gameObject.tag == "Zombie" && !monster.GetComponent<ZombiesBase>().Reverse)
                {
                    monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(dmg, Bullettype.Cattail, this._owner));
                }
            }
        }
        void CreateMao()
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject gameObject1 = PoolManage.Instance.GetPoolGameObject("Bullet", "GNCattail_bullet 1", this._owner.transform.position + Vector3.up * 0.4f + new Vector3((float)Math.Cos(i * 60 * Mathf.Deg2Rad) * 1.4f, (float)Math.Sin(i * 60 * Mathf.Deg2Rad), 0));
                gameObject1.GetComponent<BulletBase>().Init(new Vector2((float)Math.Cos(i * 60 * Mathf.Deg2Rad), (float)Math.Sin(i * 60 * Mathf.Deg2Rad)) * 5.5f, -1, UnitFaction.Plant, 40);
                gameObject1.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, i * 60);
            }
        }
        public override float SpeedChange()
        {
            return dingshen > 0 ? 0: base.SpeedChange();
        }
    }
}
