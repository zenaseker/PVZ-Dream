using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChooseUILevelChange : MonoBehaviour
{
    public void ToUp()
    {
        this.GetComponent<Canvas>().sortingLayerName = "UIUp";
    }
    public void ToUI()
    {
        this.GetComponent<Canvas>().sortingLayerName = "UI";
    }
    public void ToAlmanacs()
    {
        AlmmanacsAllbox.Instance.Init();
    }
}
