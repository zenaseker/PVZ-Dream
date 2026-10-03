using DG.Tweening;
using UnityEngine;

public class Cart : BulletBase
{
    private bool start = false;

    public override void FixedUpdate()
    {
        if (!start) return;
        this.rigidbody2d.velocity = flyspeed;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            collision.gameObject.GetComponent<ZombiesBase>().ZombieDieByCart();
            start = true;
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<Animator>().enabled = true;
            MusicManage.Instance.PlayEffect("lawnmower",1);
        }
    }

    public void Goto()
    {
        Vector3 vector3 = new Vector3(transform.position.x + 1.1f, transform.position.y, transform.position.z);
        transform.DOPath(new Vector3[] { transform.position, vector3 }, 0.4f).SetEase(Ease.Linear).onComplete += () =>
        {
            this.GetComponent<Animator>().enabled = false;
        };
    }
}
