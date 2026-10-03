using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementMap : MonoBehaviour
{
    public Transform LevelBack;
    private void OnEnable()
    {
        this.GetComponent<Image>().material.SetFloat("_MaskPower", 1f);
    }
    void Update()
    {
        this.transform.position = LevelBack.transform.position;
    }
}
