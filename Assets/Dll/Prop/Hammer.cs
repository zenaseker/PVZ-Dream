using UnityEngine;

public class Hammer : PropBase
{
    private bool inhammer = false;
    public override void Update()
    {
        if (inhammer) return;
        base.Update();
    }
    public override void OnClick()
    {
        base.OnClick();
        inhammer = true;
        this.GetComponent<Animator>().Play("Hammer");
    }
    private void AfterAnimator()
    {
        MusicManage.Instance.PlayEffect("bonk", 1f);
        bool hited = false;
        foreach (var monster in Physics2D.OverlapCircleAll(transform.position, 0.7f, 2))
        {
            if (monster.gameObject.tag == "Zombie")
            {
                monster.GetComponent<ZombiesBase>().TakeDamage(new DamageObject(1800,Bullettype.None, null)
                {
                    IgnoreArmor = true,
                });
                hited = true;
            }
        }
        if (hited)
        { 
            PropManage.Instance.OnPropUsed();
        }
        PropManage.Instance.CancelProp();
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    public override void OnShow()
    {
        base.OnShow();
        inhammer = false;
    }
}
