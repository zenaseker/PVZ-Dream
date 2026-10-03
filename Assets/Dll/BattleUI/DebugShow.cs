using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugShow : Singleton<DebugShow>
{
    private float deltatime = 5f;
    public TextMeshProUGUI textMeshProUGUI;
    public void Update()
    {
        this.deltatime -= Time.deltaTime;
        if( deltatime < 0)
        {
            Hide();
        }
    }
    public void Init(object show,float deltatime = 5f)
    {
        gameObject.SetActive(true);
        textMeshProUGUI.text = show.ToString();
        this.deltatime = deltatime;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
