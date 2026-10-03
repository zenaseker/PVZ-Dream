using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieToggle : MonoBehaviour
{
    public int id;
    public CustomLevelPanel customLevelPanel;
    public void ChangeUse(bool use)
    {
        customLevelPanel.SetZombieID(id,use);
    }
    public void SetUse(bool use)
    {
        this.GetComponent<Toggle>().isOn = use;
    }
}
