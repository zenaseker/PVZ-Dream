using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GridKey
{
    Day,//ÎÞ(°×Ìì)
    Snow,//³õÑ©
    Night,//ºÚÒ¹
    Cemetery,//Ä¹µØ
    Dream,//ÃÎ
    Pool,//Ë®³Ø
    NightPool,//ºÚÒ¹Ë®³Ø
    Roof,//ÎÝ¶¥
}
public class GridNode : MonoBehaviour
{
    public Vector2Int vector;
    public GridKey gridKey = GridKey.Day;
}
