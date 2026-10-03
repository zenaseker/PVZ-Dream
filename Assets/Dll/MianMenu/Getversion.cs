using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Getversion : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<TextMeshProUGUI>().text = "°æ±¾ºÅ£ºV" + Application.version;
    }
}
