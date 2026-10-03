
using UnityEngine;

public class Plant_IcyblastCherryBomb : Plant_CherryBomb
{
    public override void OnAttack()
    {
        Invoke("Die", 0.1f);
        GameObject obj2 = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "IceCherryBroomPS", this.transform.position);
        RandomUtil.AddOrGetComponent<CherryBroom>(obj2).Init(400,false,4.0f,0.5f);
        for (int i = 0; i < obj2.transform.childCount; i++)
        {
            obj2.transform.GetChild(i).localScale = Vector3.one * 2;
            ParticleSystem.MainModule mainModule = obj2.transform.GetChild(i).GetComponent<ParticleSystem>().main;
            mainModule.startColor = Color.blue;
        }
        GameObject obj = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "FrostStream", this.transform.position);
        RandomUtil.AddOrGetComponent<FrostStream>(obj).Init(2, 4.0f, 0.5f);
        InitBullet(obj.GetComponent<FrostStream>());
    }

}
