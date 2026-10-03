using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Zombie_polevaulter : ZombiesBase
{
    private bool injump = false;
    public void AfterJump()
    {
        rigidbody2d.velocity = new Vector2(0f, 0f);
        injump = false;
        unitbase = null;
        this.GetComponent<Animator>().SetBool("Attack", true);
        this.IgnoreSpecialAttack.Clear();
        transform.GetChild(0).Find("∏À«∞∂À").gameObject.SetActive(false);
        foreach (BattleUnitModel unit in Units)
        {
            if (unit is PlantBase)
            {
                (unit as PlantBase).OnZombieContact(this);
            }
        }
        ChangeAttackUnit(0f);
    }

    public override void DieAfterLoseHead()
    {
        if (injump)
        {
            this.Die();
            return;
        }
        base.DieAfterLoseHead();
    }
    public override void LoseArm()
    {
        transform.GetChild(0).Find("”“¥Û±€").GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).Find("”“¥Û±€").GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).Find("”“¥Û±€").GetComponent<SpriteRenderer>().sprite = Attribute.GetSprite("Zombie_polevaulter_outerarm_upper2");
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePolevaulterArm", transform.GetChild(0).Find("”“¥Û±€").GetChild(0).position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.3f, 0.3f);
        base.LoseArm();
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombiePolevaulterHead", transform.GetChild(0).Find("Õ∑").position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f, true);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Õ∑").gameObject.SetActive(false);
        transform.GetChild(0).Find("∏À«∞∂À").gameObject.SetActive(false);
        transform.GetChild(0).Find("◊Û¥Û±€").gameObject.SetActive(false);
    }
}
