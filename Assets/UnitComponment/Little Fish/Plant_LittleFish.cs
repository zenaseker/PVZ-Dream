using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_LittleFish : PlantBase
{
    float time = 0f;
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        time += Time.deltaTime;
        if (time >= 5f)
        {
            time = 0f;
            OnProduct(BattleManage.CreateSun(this.transform.position, 25, true));
            List<Vector2Int> vector2Ints = new List<Vector2Int>
            {
                new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1)
                , new Vector2Int(0, -1), new Vector2Int(0, 0), new Vector2Int(0, 1)
                , new Vector2Int(1, -1), new Vector2Int(1, 0), new Vector2Int(1, 1), 
            };
            if (this.XY.x <= 0)
            {
                vector2Ints.RemoveAll(x => x.x == -1);
            }
            if (this.XY.x >= MapManage.Instance.meshxy.x - 1)
            {
                vector2Ints.RemoveAll(x => x.x == 1);
            }
            if (this.XY.y <= 0)
            {
                vector2Ints.RemoveAll(x => x.y == -1);
            }
            if (this.XY.y >= MapManage.Instance.meshxy.y - 1)
            {
                vector2Ints.RemoveAll(x => x.y == 1);
            }
            Vector2Int vector2Int = RandomUtil.SelectOne(vector2Ints);
            vector2Int += this.XY;
            MapManage.Instance.meshPlants[vector2Int.x, vector2Int.y].AddMapBuf(new MapMeshUnitBuf_LittleFish(MapManage.Instance.meshPlants[vector2Int.x, vector2Int.y], PlantPosType.All), 1);
            this.HP -= 50;
            if (this.HP <= 0)
            {
                this.Die();
            }
        }
    }
    public class MapMeshUnitBuf_LittleFish : MapMeshUnitBuf
    {
        GameObject effect;
        public MapMeshUnitBuf_LittleFish(MapMeshPlant mesh, PlantPosType effectiveUnit) : base(mesh, effectiveUnit)
        {
        }
        public override bool OnTakeDamage(DamageObject damageObject)
        {
            if (this.stack > 0)
            {
                this.stack--;
                if (this.stack <= 0)
                {
                    this.Destory();
                }
                return true;
            }
            return base.OnTakeDamage(damageObject);
        }
        public override void AddStack(int stack)
        {
            base.AddStack(stack);
            if (effect == null)
            {
                effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Bubble", MapManage.Instance.meshpos[mesh.xy.x, mesh.xy.y]);
            }
        }
        public override void Destory()
        {
            if (effect != null)
            {
                PoolManage.Instance.PushGameObject(effect.name, effect);
            }
            base.Destory();
        }
    }
}
