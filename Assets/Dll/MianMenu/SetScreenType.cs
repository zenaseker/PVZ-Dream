using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetScreenType : MonoBehaviour
{
    public bool isat = false;
    public void Start()
    {
        if (isat)
        {
            this.GetComponent<TMP_Dropdown>().value = Screen.fullScreen ? 0 : 1;
        }
        else
        {
            if (Screen.width == 1920)
                this.GetComponent<TMP_Dropdown>().value = 0;
            if (Screen.width == 1860)
                this.GetComponent<TMP_Dropdown>().value = 1;
            if (Screen.width == 1600)
                this.GetComponent<TMP_Dropdown>().value = 2;
            if (Screen.width == 1280)
                this.GetComponent<TMP_Dropdown>().value = 3;
        }
    }
    public void Setscreentype(int num)
    {
        switch (num)
        {
            case 0:
                Screen.fullScreen = true;
                DebugShow.Instance.Init("È«ÆÁ",1f);
                break;
            case 1:
                Resolution[] resolutions = Screen.resolutions;
                Screen.SetResolution(Screen.width, Screen.height, false);
                Screen.fullScreen = false;
                DebugShow.Instance.Init("´°¿Ú»¯", 1f);
                break;
        }
    }
    public void Setscreentypenoat(int num)
    {
        switch (num)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1860, 1050, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
        }
    }
}
