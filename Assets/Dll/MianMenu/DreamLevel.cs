using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Attribute;

public class DreamLevel : MonoBehaviour
{
    public Transform DreamList;
    [Tooltip("¹Ø¿¨ÁÐ±í")]
    public Transform LevelList;
    public DreamElement levelnum = 0;
    private GameObject nowelement;
    private bool inelement = false;
    private Dictionary<Attribute.DreamElement, GameObject> ElementList = new Dictionary<DreamElement, GameObject>();
    public void Select()
    {
        if (Attribute.Instance.filedInfo.MianFinishLevel < 0)
        {
            return;
        }
        this.gameObject.SetActive(true);
        this.GetComponent<Animator>().Play("DreamLevel");
        levelnum = 0;
    }
    public void AfterSelect()
    {
        this.transform.Find("ElementList").gameObject.SetActive(true);
        this.transform.Find("Head").gameObject.SetActive(true);
        this.transform.Find("GoBack").gameObject.SetActive(true);
    }
    public void Hide()
    {
        if (inelement)
        {
            HideElement();
            return;
        }
        this.gameObject.SetActive(false);
        this.transform.Find("ElementList").gameObject.SetActive(false);
        this.transform.Find("Head").gameObject.SetActive(false);
        this.transform.Find("GoBack").gameObject.SetActive(false);
    }
    public void HideElement()
    {
        HideLevelList();
        nowelement.transform.GetChild(2).gameObject.SetActive(true);
        nowelement.transform.DOScale(1f, 0.5f).onComplete += () =>
        {
            nowelement.transform.GetChild(0).gameObject.SetActive(true);
            inelement = false;
            DreamList.GetChild(0).gameObject.SetActive(true);
        };
    }
    public void GoToLevelList(GameObject game, DreamElement num)
    {
        if (inelement) return;
        inelement = true;
        nowelement = game;
        levelnum = num;
        DreamList.GetChild(0).gameObject.SetActive(true);
        game.SetActive(true);
        game.transform.GetChild(0).gameObject.SetActive(false);
        game.transform.DOScale(10f, 0.5f).onComplete += () =>
        {
            game.transform.GetChild(2).gameObject.SetActive(false);
            InitLevelList();
        };
    }
    public void HideLevelList()
    {
        for (int i = 0; i < LevelList.childCount; i++)
        {
            PoolManage.Instance.PushGameObject(LevelList.GetChild(i).gameObject.name, LevelList.GetChild(i).gameObject);
        }
    }
    public void InitLevelList()
    {
        foreach (Level level in Attribute.Instance.Levels[LevelType.Dream].FindAll(x => x.Dream.Contains(levelnum)))
        {
            if (level.Dream.Count > 1 && !LevelOpen(level))
            {
                continue;
            }
            GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
            obj.transform.SetAsLastSibling();
            LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
            levelInformation.Init(level);
            obj.transform.GetChild(4).gameObject.SetActive(Attribute.Instance.filedInfo.DreamFinishLevel.Contains(level.ID));
        }
    }
    public bool LevelOpen(Level level)
    {
        foreach (Attribute.DreamElement element in level.Dream)
        {
            if (!Attribute.Instance.filedInfo.DreamFinishLevel.Contains((int)element * 100))
            {
                return false;
            }
        }
        return true;
    }
}
