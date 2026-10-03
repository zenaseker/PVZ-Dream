using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class Plant_Chomper: PlantBase
{
    protected bool ineat = false;
    protected float eatingtime = -1f;
    public ZombiesBase zombies = null;

    public override void Init(Vector2Int xy, PlantInfo plant)
    {
        base.Init(xy, plant);
    }
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        if (eatingtime >= 0f)
        {
            eatingtime -= Time.deltaTime;
            if (eatingtime <= 0f)
            {
                this.GetComponent<Animator>().SetTrigger("EatOut");
                ineat = false;
            }
            return;
        }
        RaycastHit2D raycastHit2D = BattleManager.GetRay(this.transform.position, Vector2.right, 2.7f, 2);
        if (raycastHit2D)
        {
            zombies = raycastHit2D.collider.gameObject.GetComponent<ZombiesBase>();
            if (zombies != null && !zombies.losehead && !zombies.IgnoreSpecialAttack.Contains(IgonrePlant.Chomper))
            {
                this.GetComponent<Animator>().SetTrigger("Attack");
                return;
            }
            zombies = null;
        }
    }

    public void PlayAttackMusic()
    {
        if (ineat) return;
        MusicManage.Instance.PlayEffect("bigchomp", 0.5f);
    }
    public override void OnAttack()
    {
        ineat = (zombies != null);
        this.GetComponent<Animator>().SetBool("EatSucess", ineat);
        if (ineat)
        {
            OnSucessEat(zombies);
            eatingtime = 40f - bufDetail.EatingTime;
            if (eatingtime <= 0f)
            {
                eatingtime = 0.1f;
            }
            zombies.Destroy();
            this.GetComponent<Animator>().SetBool("Attack", false);
        }
    }
    public virtual void OnSucessEat(ZombiesBase zombie)
    {

    }
    public virtual void AfterSwallow()
    {
    }
}
