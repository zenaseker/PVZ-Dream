using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Gravebuster : PlantBase
{
    public override void Init(Vector2Int xy, Attribute.PlantInfo plant)
    {
        base.Init(xy, plant);
        MusicManage.Instance.PlayEffect("gravebusterchomp", 1);
    }
    public void Eat()
    {
        MusicManage.Instance.PlayEffect("gravebutton", 1);
        if (MapManage.Instance.meshPlants[this.XY.x,this.XY.y].tombston != null)
        {
            GameObject.Destroy(MapManage.Instance.meshPlants[this.XY.x, this.XY.y].tombston.gameObject);
            MapManage.Instance.meshPlants[this.XY.x, this.XY.y].tombston = null;
        }
        this.Die();
    }
}
