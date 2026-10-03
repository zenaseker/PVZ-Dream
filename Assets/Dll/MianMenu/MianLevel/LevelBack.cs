using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelBack : MonoBehaviour, IPointerClickHandler
{
    public Sprite[] textures;
    public Sprite[] dreamtextures;
    public LevelBackHideGameObjects[] LevelAndLine;
    public GameObject MianLevel;
    public GameObject ElementMap;
    public Transform ElementButton;
    public DreamLevelMpa dreamLevelMpa;
    int nowTexture = 0;
    bool inchange = false;
    bool indream = false;
    [System.Serializable]
    public class LevelBackHideGameObjects
    {
        public GameObject[] Objs;
    }
    public void OnEnable()
    {
        inchange = false;
        this.GetComponent<Image>().material.SetTexture("_GotoTex", textures[0].texture);
        foreach (GameObject obj in LevelAndLine[0].Objs)
        {
            obj.SetActive(true);
        }
        for (int i = 1;i < Attribute.Instance.filedInfo.MianFinishLevel + 1; i ++)
        {
            if (i > LevelAndLine.Length) return;
            foreach(GameObject obj in LevelAndLine[i].Objs)
            {
                obj.SetActive(true);
            }
        }
    }
    public void ChangeCenter(Transform transform)
    {
        Vector3 vector3 = Camera.main.WorldToViewportPoint(transform.position);
        vector3.z = 0;
        vector3.x -= 0.5f;
        vector3.y -= 0.5f;
        vector3 *= -0.3f;
        ElementMap.GetComponent<Image>().material.SetVector("_EdgePos", new Vector4(vector3.x, vector3.y, 0, 0));
    }
    public void ChangeMap(int num)
    {
        if (inchange) return;
        MusicManage.Instance.PlayEffect("DreamEnter", 0.5f);
        inchange = true;
        indream = !indream;
        ElementMap.GetComponent<Image>().sprite = dreamtextures[num];
        if (indream)
        {
            ElementMap.GetComponent<Image>().material.DOFloat(0.5f, "_MaskPower", 0.5f).onComplete += InDream;
            dreamLevelMpa.Init(num + 1);
        }
        else
        {
            ElementMap.GetComponent<Image>().material.SetFloat("_MaskPower", 0.5f);
            ElementMap.GetComponent<Image>().material.DOFloat(1, "_MaskPower", 0.5f).onComplete += OutDream;
            dreamLevelMpa.Hide();
        }
    }
    public void InDream()
    {
        inchange = false;
        ElementMap.GetComponent<Image>().material.SetFloat("_MaskPower", 0);
    }
    public void OutDream()
    {
        inchange = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ShowLevelInfo.Instance.Hide();
    }
    public void OnChange(Vector2 vector2)
    {
        if (vector2.x < 0.7f)
        {
            if (vector2.y > 0.7f)
            {
                if (nowTexture != 0)
                {
                    ChangeTexture(0);
                }
            }
            else
            {
                if (nowTexture != 3)
                {
                    ChangeTexture(3);
                }
            }
        }
        else
        {
            if (vector2.y > 0.7f)
            {
                if (nowTexture != 1)
                {
                    ChangeTexture(1);
                }
            }
            else
            {
                if (nowTexture != 2)
                {
                    ChangeTexture(2);
                }
            }
        }
    }
    public void ChangeTexture(int num)
    {
        nowTexture = num;
        this.GetComponent<Image>().material.SetTexture("_GotoTex", textures[num].texture);
        if (!inchange)
        {
            inchange = true;
            this.GetComponent<Image>().material.SetFloat("_Goto", -1);
            this.GetComponent<Image>().material.DOFloat(1f, "_Goto", 0.5f).onComplete += ChangeTextureAfterChange;
        }
    }
    void ChangeTextureAfterChange()
    {
        this.GetComponent<Image>().sprite = textures[nowTexture];
        this.GetComponent<Image>().material.SetFloat("_Goto", 1);
        inchange = false;
    }
}
