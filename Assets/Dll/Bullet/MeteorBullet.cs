using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour
{
    public void OnEnable()
    {
        MusicManage.Instance.PlayEffect("∑ÁII", 1);
        RandomUtil.AddOrGetComponent<TimeDestory>(this.gameObject).Init(2f);
    }
    public void Hit()
    {
        MusicManage.Instance.PlayEffect("Boom", 1);
        foreach (var monster in Physics2D.OverlapCircleAll(this.transform.position, 4f, 2))
        {
            if (monster.gameObject.tag == "Zombie")
            {
                monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(300, Bullettype.Meteor, null)
                {
                    DamageElement = DamageElement.Fire,
                    IsBroom = true,
                });
            }
        }
    }
}
