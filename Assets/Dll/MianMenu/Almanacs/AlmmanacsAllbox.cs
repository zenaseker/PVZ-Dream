using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmmanacsAllbox : Singleton<AlmmanacsAllbox>
{
    public void Init()
    {
        this.gameObject.SetActive(true);
        this.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(1).GetComponent<Canvas>().worldCamera = Camera.main;
        this.transform.GetChild(2).GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
