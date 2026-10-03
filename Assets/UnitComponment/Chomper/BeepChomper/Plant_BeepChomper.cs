using UnityEngine;

public class Plant_BeepChomper : Plant_Chomper
{
    int eatcount = 0;
    bool isPossess = false;
    public override void OnAttack()
    {
        ineat = zombies != null;
        this.GetComponent<Animator>().SetBool("EatSucess", ineat);
        if (ineat)
        {
            OnSucessEat(zombies);
            float time = eatcount * 10 + 20;
            eatingtime = time - bufDetail.EatingTime;
            if (eatingtime <= 0f)
            {
                eatingtime = 0.1f;
            }
            zombies.Destroy();
            this.GetComponent<Animator>().SetBool("Attack", false);
        }
    }
    public override void OnSucessEat(ZombiesBase zombie)
    {
        if (zombie.bufDetail.GetKeyWordBuf(KeyWordBuf.Possess) != null)
        {
            isPossess = true;
            eatcount = 0;
        }
        else
        {
            isPossess = false;
            eatcount++;
        }
    }
    public override void AfterSwallow()
    {
        this.GetComponent<Animator>().SetBool("HasPossess", isPossess);
    }
    public void Fight()
    {
        MusicManage.Instance.PlayEffect("BeepChomperFight 1", 0.5f);
        Vector3 pos = this.transform.position + Vector3.up * 0.5f;
        GameObject effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "BeepChomperFight", pos);
        RandomUtil.AddOrGetComponent<TimeDestory>(effect).Init(0.5f);
        RaycastHit2D[] raycastHit2D = BattleManager.GetRayAll(pos, Vector2.right, 10, 2);
        if (raycastHit2D != null && raycastHit2D.Length > 0)
        {
            foreach (RaycastHit2D ray in raycastHit2D)
            {
                if (ray.transform.tag == "Zombie" && !ray.transform.GetComponent<ZombiesBase>().IgnoreSpecialAttack.Contains(IgonrePlant.LineAttacker))
                {
                    ZombiesBase zombies = ray.transform.GetComponent<ZombiesBase>();
                    zombies.TakeDamage(new DamageObject(160, Bullettype.Chomper, this) { DamageElement = DamageElement.Soul});
                }
            }
        }
    }
}
