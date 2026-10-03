using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_PhantomShroom : Plant_HypnosShroom
{
    float buftime = 0f;
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        buftime += Time.deltaTime;
        if (buftime >= 30f)
        {
            this.bufDetail.AddKeyWordBuf(KeyWordBuf.MoonErosion, 1);
            buftime = 0f;
        }
    }
    public override void OnControl(ZombiesBase zombie)
    {
        base.OnControl(zombie);
        zombie.bufDetail.AddBuf(this.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion));
        zombie.bufDetail.AddBuf(new BattleUnitBuf_Phantom(), 1);
    }

    public class BattleUnitBuf_Phantom : BattleUnitBuf
    {
        float time = 0f;
        public override int MaxHp()
        {
            return 50;
        }
        public override void OnUpdate(float deltatime)
        {
            base.OnUpdate(deltatime);
            time += Time.deltaTime;
            if (time >= 5f)
            {
                if (this._owner.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion) != null)
                {
                    this._owner.ReCoverHp(10 * this._owner.bufDetail.GetKeyWordBuf(KeyWordBuf.MoonErosion).stack);
                }
                time = 0f;
            }
        }
    }
}
