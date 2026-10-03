using System;
using TMPro;
using UnityEngine;
using static Attribute;


public class Challenge : MonoBehaviour
{
    public TextMeshProUGUI meshPro;
    public GameObject Head;
    public Transform LevelList;

    public void InitAdventure()
    {
        if (Attribute.Instance.filedInfo.MianFinishLevel == -1)
        {
            Attribute.Level level = Attribute.Instance.Levels[LevelType.Adventure][0];
            GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
            obj.transform.SetAsLastSibling();
            LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
            levelInformation.Init(level);
            return;
        }
        for (int i = 0;i <= Attribute.Instance.filedInfo.MianFinishLevel+1; i++)
        {
            if (i < Attribute.Instance.Levels[LevelType.Adventure].Count)
            {
                Attribute.Level level = Attribute.Instance.Levels[LevelType.Adventure][i];
                GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
                obj.transform.SetAsLastSibling();
                LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
                levelInformation.Init(level);
                obj.transform.GetChild(4).gameObject.SetActive(Attribute.Instance.filedInfo.MianFinishLevel >= level.ID);
            }
        }
    }
    public void InitChallenge()
    {
        foreach (Attribute.Level level in Attribute.Instance.Levels[LevelType.Challenge])
        {
            GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
            obj.transform.SetAsLastSibling();
            LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
            levelInformation.Init(level);
            obj.transform.GetChild(4).gameObject.SetActive(Attribute.Instance.filedInfo.ChallengeFinishLevel.Contains(level.ID));
        }
    }
    public void InitRiddle()
    {
        foreach (Attribute.Level level in Attribute.Instance.Levels[LevelType.LittleGame])
        {
            GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
            obj.transform.SetAsLastSibling();
            LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
            levelInformation.Init(level);
            obj.transform.GetChild(4).gameObject.SetActive(Attribute.Instance.filedInfo.LittleGameFinishLevel.Contains(level.ID));
        }
    }
    public void InitLife()
    {
        foreach (Attribute.Level level in Attribute.Instance.Levels[LevelType.Life])
        {
            GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
            obj.transform.SetAsLastSibling();
            LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
            levelInformation.Init(level);
            obj.transform.GetChild(4).gameObject.SetActive(Attribute.Instance.filedInfo.LifeFinishLevel.Contains(level.ID));
        }
    }
    public void InitOther()
    {
        foreach (Attribute.Level level in Attribute.Instance.Levels[LevelType.Other])
        {
            GameObject obj = PoolManage.Instance.GetPoolGameObject("Level", "Level", LevelList);
            obj.transform.SetAsLastSibling();
            LevelInformation levelInformation = obj.GetComponent<LevelInformation>();
            levelInformation.Init(level);
            obj.transform.GetChild(4).gameObject.SetActive(Attribute.Instance.filedInfo.OtherFinishLevel.Contains(level.ID));
        }
    }





    public void HideLevelList()
    {
        for (int i = 0; i < LevelList.childCount; i++)
        {
            PoolManage.Instance.PushGameObject(LevelList.GetChild(i).gameObject.name, LevelList.GetChild(i).gameObject);
        }
        this.gameObject.SetActive(false);
    }



    public void GetLevelList(int type)
    {
        this.gameObject.SetActive(true);
        switch (type)
        {
            case 0:
                meshPro.text = "冒险模式";
                this.InitAdventure();
                break;
            case 1:
                meshPro.text = "挑战模式";
                this.InitChallenge();
                break;
            case 2:
                meshPro.text = "小游戏";
                this.InitRiddle();
                break;
            case 3:
                meshPro.text = "生存模式";
                this.InitLife();
                break;

        }
    }
}
