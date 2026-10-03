
using UnityEngine;

public class Plant_CherryBomb : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        this.GetComponent<Animator>().Play("Broom");
    }
    public override void TakeDamage(DamageObject damageObject)
    {
        return;
    }
    public override void OnAttack()
    {
        Invoke("Die", 0.1f);
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "CherryBroomPS", this.transform.position);
        RandomUtil.AddOrGetComponent<CherryBroom>(obj).Init(1800, true,2.4f,0.5f);
        InitBullet(gameObject.GetComponent<IEnchantment>());
    }
    public void AfterBroom()
    {
        this.Die();
    }

}
