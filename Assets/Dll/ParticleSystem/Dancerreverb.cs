using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancerreverb : MonoBehaviour
{
    public int Line = -1;
    float time = 0f;
    public void Init(int Line)
    {
        this.Line = Line;
        time = 0f;
    }
    private void FixedUpdate()
    {
        if (BattleManage.Instance != null && BattleManage.Instance.battleStage == BattleStage.End)
        {
            PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
        }
        time += Time.fixedDeltaTime;
        if (time >= 60f)
        {
            Create();
            time = 0f;
        }
    }
    public ZombiesBase Create()
    {
        if (this.Line == -1) return null;
        PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ObjectCreate", this.transform.position);
        GameObject obj = ZombieManage.Instance.InitZombie(7, this.Line);
        ZombieManage.Instance.LoadZombieMess(obj, 7, Line);
        obj.transform.position = this.transform.position;
        obj.GetComponent<Animator>().SetBool("Go", true);
        return obj.GetComponent<ZombiesBase>();
    }
}
