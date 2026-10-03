using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class ElelmentLevelBase : MonoBehaviour
{
    public DreamElement DreamLevel = DreamElement.Default;
    public Vector2 movespeed = Vector2.zero;
    public float maxline = 0f;
    float line = 0f;
    public DreamLevel dreamLevel = null;
    public void OnClick()
    {
        dreamLevel.GoToLevelList(this.gameObject, DreamLevel);
    }
    void Update()
    {
        line += Time.deltaTime * maxline;
        if (line > 1f)
        {
            movespeed *= -1;
            line = 0f;
        }
        this.transform.localPosition += (Vector3)movespeed;
    }
}
