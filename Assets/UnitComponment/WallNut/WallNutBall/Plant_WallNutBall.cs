using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_WallNutBall : PlantBase
{
    int movedirection = 0;
    int zombieline = -1;
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        MapManage.Instance.meshPlants[XY.x, XY.y].AddPlant(null, PlantPosType.Default);
    }
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(2f, movedirection * 2.5f);
        if (this.transform.position.x > 10)
        {
            this.Destory();
        }
        else if (this.transform.position.y <= MapManage.Instance.meshpos[0, 0].y - 0.5)
        {
            movedirection = 1;
        }
        else if (this.transform.position.y >= MapManage.Instance.meshpos[MapManage.Instance.meshxy.x - 1, 0].y + 0.5)
        {
            movedirection = -1;
        }
    }
    public override void TakeDamage(DamageObject damageObject)
    {
        return;
    }
    public override void OnZombieContact(ZombiesBase zombiesBase)
    {
        if (zombieline == zombiesBase.Line) return;
        MusicManage.Instance.PlayEffect(RandomUtil.SelectOne(Attribute.Instance.NormalMusic["bowling"]),0.5f);
        zombiesBase.TakeDamage(new DamageObject(600,Bullettype.WallNutBall,this));
        zombieline = zombiesBase.Line;
        if (movedirection == 0)
        {
            movedirection = Random.value > 0.5f ? 1 : -1;
        }
        else if(zombieline <= 0)
        {
            movedirection = 1;
        }
        else if (zombieline >= MapManage.Instance.meshxy.x - 1)
        {
            movedirection = -1;
        }
        else
        {
            movedirection = - movedirection;
        }

    }
}
