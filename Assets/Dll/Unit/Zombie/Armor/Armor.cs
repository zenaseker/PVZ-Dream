using DG.Tweening;
using UnityEngine;

public enum ArmorType
{
    None,//ÎÞ
    Iron,//ÌúÆ÷
}

//·À¾ß
public class Armor : MonoBehaviour
{
    public int MaxDurable;

    public ArmorType type = ArmorType.None;

    protected int Durable;

    protected ZombiesBase ZombiesBase;

    public int armortype;

    public SpriteRenderer aromorspr;

    public Sprite tex0 = null;
    public Sprite tex1 = null;
    public Sprite tex2 = null;

    public virtual void Init(ZombiesBase zombiesBase)
    {
        Durable = MaxDurable;
        ZombiesBase = zombiesBase;
    }

    public virtual void TakeDamage(DamageObject damageObject)
    {
        aromorspr.material.DOFloat(1.3f, "_HighLight", 0.05f).onComplete += () =>
        {
            aromorspr.material.DOFloat(1f, "_HighLight", 0.05f);
        };
        this.Durable -= (int)(damageObject.Damage * DamageReDuction(damageObject.Damage, damageObject.DamageElement));
        this.ChangeSprite((float)Durable / (float)MaxDurable);
        if (this.Durable <= 0)
        {
            this.Destory();
        }
    }
    public virtual void ChangeSprite(float hp)
    {
        if (hp <= 0.666f)
        {
            if (hp <= 0.333f)
            {
                aromorspr.sprite = tex2;
                return;
            }
            aromorspr.sprite = tex1;
        }
    }
    public virtual float DamageReDuction(int dmg,DamageElement type)
    {
        return 1f;
    }
    public virtual void Destory()
    {
        ZombiesBase.DestoryArmor(this);
        DOTween.Kill(aromorspr.material,true);
        DOTween.Kill(aromorspr.color, true);
        this.transform.SetParent(null);
        this.transform.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, -90f), 0.499f);
        aromorspr.material.DOFloat(0f, "_HighLight", 0.499f);
        this.transform.GetChild(0).DOPath(new Vector3[] { this.transform.GetChild(0).position, new Vector3(this.transform.GetChild(0).position.x + 0.6f, this.transform.GetChild(0).position.y - 0.4f, this.transform.GetChild(0).position.z), new Vector3(this.transform.GetChild(0).position.x + 1f, this.transform.GetChild(0).position.y - 1f, this.transform.GetChild(0).position.z) }
        , 0.5f).SetEase(Ease.OutQuad).onComplete += () =>
        {
            DOTween.Kill(this.transform, true);
            DOTween.Kill(aromorspr, true);
            GameObject.Destroy(this.transform.gameObject);
        };

    }
}