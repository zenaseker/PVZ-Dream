using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAttention : MonoBehaviour
{
    public void Skipattention(string www)
    {
        Application.OpenURL("https://space.bilibili.com/" + www);
    }
}
