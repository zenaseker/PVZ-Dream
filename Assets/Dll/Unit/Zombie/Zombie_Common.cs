using DG.Tweening;
using UnityEngine;

public class Zombie_Common : ZombiesBase
{

    public override void LoseArm()
    {
        base.LoseArm();
        transform.GetChild(0).Find("×ó¼ç").GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).Find("×ó¼ç").GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).Find("×ó¼ç").GetComponent<SpriteRenderer>().sprite = Attribute.GetSprite("Zombie_outerarm_upper2");
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieCommonArm", transform.GetChild(0).Find("×ó¼ç").GetChild(0).position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.6f, 0.6f);
    }
    public override void LoseHead()
    {
        base.LoseHead();
        Transform game = transform.GetChild(0).Find("Í·");
        Transform head = PoolManage.Instance.GetPoolGameObject("ZombieBody", "ZombieCommonHead", game.position).transform;
        RandomUtil.AddOrGetComponent<ZombieBody>(head.gameObject).Init(this.Line);
        float startspeed = Random.Range(0.5f, 1.5f);
        head.GetComponent<Rigidbody2D>().velocity = new Vector2(startspeed, 5f);
        head.gameObject.AddComponent<TimeDestory>().Init(1f);
        head.transform.DOLocalRotate(new Vector3(0f, 0f, 180f * startspeed), 0.58f);
        transform.GetChild(0).Find("Í·").gameObject.SetActive(false);
    }
}
