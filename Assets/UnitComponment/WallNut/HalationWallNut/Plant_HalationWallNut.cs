using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class Plant_HalationWallNut : Plant_WallNut
{
    private float detlatime = 5f;
    protected override void OnPlantUpdate()
    {
        base.OnPlantUpdate();
        this.detlatime -= Time.deltaTime;
        if (this.detlatime < 0)
        {
            int num = this.component._matrix._RegetMatrixnum2;
            this.ReCoverHp(50 + ((num > 8)?400:num * 50));
            num = this.component._matrix._RegetMatrixnum2;
            this.detlatime = 5f - 0.5f * ((num > 8)?4:num * 0.5f);
        }
        this.GetComponent<Animator>().SetFloat("Speed", 1 + this.component._matrix._RegetMatrixnum2 * 0.25f);
    }
}
