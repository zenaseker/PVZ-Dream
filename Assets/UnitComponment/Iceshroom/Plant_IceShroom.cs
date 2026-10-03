using UnityEngine;

public class Plant_IceShroom : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        Invoke("Broom", 1f);
    }
    public override void OnTakeDamage(int dmg, BattleUnitModel attacker, DamageElement damageType = DamageElement.Default)
    {
        return;
    }
    public void Broom()
    {
        Invoke("Die", 0.1f);
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "IceShroomExplode",this.transform.position);
        RandomUtil.AddOrGetComponent<TimeDestory>(gameObject).Init(1f);
        for(int i = 0;i < GameObject.Find("ZombieManage").transform.childCount; i++)
        {
            if (GameObject.Find("ZombieManage").transform.GetChild(i).GetComponent<ZombiesBase>().bufDetail.GetKeyWordBuf(KeyWordBuf.MindControl) == null)
            {
                GameObject.Find("ZombieManage").transform.GetChild(i).GetComponent<ZombiesBase>().bufDetail.AddKeyWordBuf(KeyWordBuf.Cold, 4, 4);
            }
        }
        MusicManage.Instance.PlayEffect("frozen", 1);

    }
}
