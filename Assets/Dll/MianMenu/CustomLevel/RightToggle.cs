using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RightToggle : MonoBehaviour
{
    public string id;
    public CustomLevelPanel customLevelPanel;
    public void ChangeToggle(bool ison)
    {
        customLevelPanel.SetCanUseProp(id, ison);
        ChangeShow(ison);
    }
    public void SetUse(bool use)
    {
        this.GetComponent<Toggle>().isOn = use;
        ChangeShow(use);
    }
    public void ChangeShow(bool ison)
    {
        if (ison)
        {
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("½ûÖ¹", "ÔÊÐí");
        }
        else
        {
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("ÔÊÐí", "½ûÖ¹");
        }
    }
}
