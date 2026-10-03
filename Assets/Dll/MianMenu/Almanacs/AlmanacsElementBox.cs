using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Attribute;

public class AlmanacsElementBox : MonoBehaviour
{
    AlmanacsByPlant almanacsByPlant;
    Attribute.DreamElement id;
    bool inuse = false;
    public void Init(AlmanacsByPlant almanacs, DreamElement ID)
    {
        almanacsByPlant = almanacs;
        id = ID;
        this.GetComponent<Button>().onClick.AddListener(OnCilck);
        this.transform.GetChild(1).gameObject.SetActive(!inuse);
    }
    public void OnCilck()
    {
        almanacsByPlant.OnCilck(id);
        inuse = !inuse;
        this.transform.GetChild(1).gameObject.SetActive(!inuse);
    }
}
