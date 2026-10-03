using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class ComponentBase
{
    public ComponentBase Clone()
    {
        return (ComponentBase)MemberwiseClone();
    }
}