
using UnityEngine;

public class Plant_WallNut : PlantBase
{
    public Sprite WallNut1;
    public Sprite WallNut2;
    public Sprite WallNut3;
    float damagedefaulttime = 2f;
    public override void OnTakeDamage(int damage, BattleUnitModel attacker,DamageElement damageType)
    {
        this.damagedefaulttime = 2f;
        transform.GetComponent<Animator>().enabled = false;
    }
    public override void OnHpChange(int num)
    {
        base.OnHpChange(num);
        if (this.HPFloat < 0.666f)
        {
            if (this.HPFloat < 0.333f)
            {
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WallNut3;
                return;
            }
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WallNut2;
            return;
        }
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WallNut1;
    }
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        this.damagedefaulttime -= Time.deltaTime;
        if (damagedefaulttime < 0)
        {
            transform.GetComponent<Animator>().enabled = true;
        }
    }

}
