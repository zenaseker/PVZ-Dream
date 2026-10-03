using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Attribute;

public class CustomLevelPanel : MonoBehaviour
{
    public Attribute.Level level;
    public Transform Zombiespos;
    public void OnEnable()
    {
        level = new Attribute.Level();
        level.Type = LevelType.Challenge;
        level.KeyImage = "CustomLevel";
        level.unclockplantid = new List<int> { 0 };
        level.plantmesh = level.Map = "Day";
        level.Music = "loon";
        foreach (ZombieInfo info in Attribute.ZombieInfos.ZombieInfoes)
        {
            if (info.IsCreateZombie) continue;
            Attribute.ZombieDescribe zombieDescribe = info.zombieDescribe;
            GameObject zombie = GameObject.Instantiate<GameObject>(Zombiespos.GetChild(0).gameObject, Zombiespos);
            zombie.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = zombieDescribe.Name;
            zombie.GetComponent<ZombieToggle>().id = info.ID;
        }
        Zombiespos.GetChild(0).gameObject.SetActive(false);
    }

    public void Load()
    {
        if (Attribute.Instance.GetLevel(LevelType.Challenge, level.ID) != null)
        {
            level = Attribute.Instance.GetLevel(LevelType.Challenge, level.ID).Clone();
            WriteInfo();
            DebugShow.Instance.Init("读取成功: " + Attribute.Instance.GetLevel(LevelType.Challenge, level.ID).Name);
        }
        else
        {
            DebugShow.Instance.Init("不存在该ID的关卡");
        }
    }
    int GetValue(string name)
    {
        switch (name)
        {
            case "Day":
                return 0;
            case "DayOneLine":
                return 1;
            case "Winter":
                return 2;
            case "Night":
                return 3;
            case "Cemetery":
                return 4;
            case "WallNutBall":
                return 5;
        }
        return 0;
    }
    string GetString(int value)
    {
        switch (value)
        {
            case 0:
                return "Day";
            case 1:
                return "DayOneLine";
            case 2:
                return "Winter";
            case 3:
                return "Night";
            case 4:
                return "Cemetery";
            case 5:
                return "WallNutBall";
        }
        return "Day";
    }
    public void WriteInfo()
    {
        this.transform.Find("Left").Find("Head_ID").Find("Input").GetComponent<TMP_InputField>().text = level.ID.ToString();
        this.transform.Find("Left").Find("Head_Name").Find("Input").GetComponent<TMP_InputField>().text = level.Name.ToString();
        this.transform.Find("Left").Find("Head_Image").Find("Input").GetComponent<TMP_InputField>().text = level.KeyImage;
        this.transform.Find("Left").Find("Head_Map").Find("Dropdown").GetComponent<TMP_Dropdown>().value = GetValue(level.Map);
        this.transform.Find("Left").Find("Head_GirdKey").Find("Dropdown").GetComponent<TMP_Dropdown>().value = (int)level.girdKey;
        this.transform.Find("Left").Find("Head_LevelIcons").Find("Dropdown").GetComponent<TMP_Dropdown>().value = (int)level.LevelIcons;
        this.transform.Find("Left").Find("Head_StartSunNumber").Find("Input").GetComponent<TMP_InputField>().text = level.StartSunnumber.ToString();
        this.transform.Find("Left").Find("Head_ZombieCountPower").Find("Input").GetComponent<TMP_InputField>().text = level.ZombieCountPower.ToString();
        this.transform.Find("Left").Find("Head_StartTime").Find("Input").GetComponent<TMP_InputField>().text = level.StartTime.ToString();
        this.transform.Find("Left").Find("Head_FlagCount").Find("Input").GetComponent<TMP_InputField>().text = level.FlagCount.ToString();
        this.transform.Find("Left").Find("Head_TombstonNum").Find("Input").GetComponent<TMP_InputField>().text = level.Tombston.ToString();
        for(int i = 1;i < Zombiespos.childCount; i++)
        {
            ZombieToggle toggle = Zombiespos.transform.GetChild(i).GetComponent<ZombieToggle>();
            toggle.SetUse(level.ZombiesID.Contains(toggle.id));
        }
        this.transform.Find("Right").Find("Head_NotUseProp").GetComponent<RightToggle>().SetUse(level.CanUseProp);
        this.transform.Find("Right").Find("Head_CanNeatureSun").GetComponent<RightToggle>().SetUse(level.CanCreateSun);
    }
    public void Save()
    {
        if (level.ZombiesID.Count <= 0)
        {
            level.ZombiesID.Add(0);
        }
        SaveLoadManager.Save<Level>(level.Name, level, ".lev");
        if (Attribute.Instance.Levels[LevelType.Challenge].Find(x => x.ID == level.ID) != null)
        {
            Attribute.Instance.Levels[LevelType.Challenge].RemoveAll(x => x.ID == level.ID);
        }
        Attribute.Instance.Levels[LevelType.Challenge].Add(level);
        DebugShow.Instance.Init("保存成功!");
    }
    public void SetZombieID(int id,bool use)
    {
        if (use)
        {
            if (level.ZombiesID.Contains(id))
            {
                return;
            }
            level.ZombiesID.Add(id);
        }
        else
        {
            if (!level.ZombiesID.Contains(id))
            {
                return;
            }
            level.ZombiesID.Remove(id);
        }
    }

    public void SetCanUseProp(string type, bool ison)
    {
        if (type == "prop")
        {
            level.CanUseProp = ison;
        }
        if (type == "sun")
        {
            level.CanCreateSun = ison;
        }
    }
    public void SetID(string id)
    {
        level.ID = int.Parse(id);
    }
    public void SetName(string id)
    {
        level.Name = id;
    }
    public void SetImage(string id)
    {
        level.KeyImage = id;
    }
    public void SetMap(int id)
    {
        level.Map = GetString(id);
        level.plantmesh = GetString(id);
        level.Music = GetString(id);
    }
    public void SetGirdKey(int id)
    {
        level.girdKey = (GridKey)id;
    }
    public void SetLevelIcon(int id)
    {
        level.LevelIcons = (LevelIcon)id;
    }
    public void SetStartSunNumber(string id)
    {
        level.StartSunnumber = int.Parse(id);
    }
    public void SetZombieCountPower(string id)
    {
        level.ZombieCountPower = float.Parse(id);
    }
    public void SetStartTime(string id)
    {
        level.StartTime = float.Parse(id);
    }
    public void SetFlagCount(string id)
    {
        level.FlagCount = int.Parse(id);
    }
    public void SetTombstonNum(string id)
    {
        level.Tombston = int.Parse(id);
    }
}
