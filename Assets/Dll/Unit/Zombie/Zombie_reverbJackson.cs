using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_reverbJackson : Zombie_Jackson
{
    bool first = false;
    protected override void AnimSummon()
    {
        if (!first)
        {
            GameObject Dancerreverb = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "Dancerreverb", this.transform.position);
            Dancerreverb.GetComponent<Dancerreverb>().Init(Line);
            first = true;
        }
        this.GetComponent<Animator>().SetBool("loseDancer", false);
        if (dancer[0] == null && this.Line != 0)
        {
            CreateParticle(0,408);
        }
        if (dancer[1] == null)
        {
            CreateParticle(1, 408);
        }
        if (dancer[2] == null)
        {
            CreateParticle(2, 408);
        }
        if (dancer[3] == null && this.Line != MapManage.Instance.meshxy.x - 1)
        {
            CreateParticle(3, 408);
        }
    }

}
