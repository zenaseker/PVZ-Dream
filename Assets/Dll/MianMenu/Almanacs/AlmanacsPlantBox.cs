using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlmanacsPlantBox : MonoBehaviour
{
    AlmanacsByPlant almanacsByPlant;
    public int id;
    public void Init(AlmanacsByPlant almanacs, int ID)
    {
        almanacsByPlant = almanacs;
        id = ID;
        this.GetComponent<Button>().onClick.AddListener(OnCilck);
    }
    public void OnCilck()
    {
        almanacsByPlant.OnCilck(id);
    }
}
