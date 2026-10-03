using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnateBase : ComponentBase
{
    [HideInInspector] public ComponentDetail _detail;
    public virtual void Init(ComponentDetail detail)
    {
        _detail = detail;
    }
    public virtual void Destory()
    {

    }
}
