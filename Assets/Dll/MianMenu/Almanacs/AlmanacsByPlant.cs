using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Attribute;

public class AlmanacsByPlant : MonoBehaviour
{
    public Transform PlantPos;
    public TextMeshProUGUI PlantName;
    public TextMeshProUGUI PlantDescription;
    public Transform BoxList;
    public Transform ElementList;
    GameObject showplant = null;
    bool isInit = false;
    List<Attribute.DreamElement> nowlist = new List<Attribute.DreamElement>();
    List<GameObject> cardlist = new List<GameObject>();

    void Start()
    {
        if (isInit) return;
        List<Attribute.DreamElement> dreamElements = new List<Attribute.DreamElement>();//读取已解锁植物
        List<int> orginplantid = new List<int>();
        cardlist = new List<GameObject>();
        foreach (int id in Attribute.Instance.filedInfo.Unclockplantid)
        {
            if (Attribute.Instance.GetPlantInfo(id) == null)
            {
                DebugShow.Instance.Init("存在未收录的植物，请联系制作者！");
                continue;
            }
            if (Attribute.Instance.GetPlantInfo(id).plantDescribe == null)
            {
                DebugShow.Instance.Init("存在未写入信息的植物，请联系制作者！");
                continue;
            }
            if (!orginplantid.Contains(id % 100))
            {
                orginplantid.Add(id % 100);
            }
            if (Attribute.Instance.GetPlantInfo(id).DreamElement.Contains(Attribute.DreamElement.Dream))
            {
                orginplantid.Add(id);
            }
            foreach (Attribute.DreamElement element in Attribute.Instance.GetPlantInfo(id).DreamElement)
            {
                if (!dreamElements.Contains(element))
                {
                    dreamElements.Add(element);
                }
            }
        }
        BoxList.GetComponent<GridLayoutGroup>().enabled = true;
        foreach (int id in orginplantid)//加载植物卡片
        {
            if (id == 99 || id == 503) { continue; }
            GameObject box = PoolManage.Instance.GetPoolGameObject("SeedCard", Attribute.Instance.GetPlantInfo(id).CardPrefab.name, BoxList);
            RandomUtil.AddOrGetComponent<AlmanacsPlantBox>(box).Init(this, id);
            cardlist.Add(box);
        }
        foreach (Attribute.DreamElement element in dreamElements)//加载元素卡片
        {
            if (element == Attribute.DreamElement.Default || element == Attribute.DreamElement.Dream){ continue; }
            GameObject box = PoolManage.Instance.GetPoolGameObject("SeedElement", "ElementSeed_" + element.ToString(), ElementList);
            GameObject.Destroy(box.GetComponent<ElementSeed>());
            RandomUtil.AddOrGetComponent<AlmanacsElementBox>(box).Init(this, element);
        }
        isInit = true;
    }

    public void OnCilck(int id)//点击植物卡片
    {
        BoxList.GetComponent<GridLayoutGroup>().enabled = false;
        if (showplant != null)//缓存植物
        {
            PoolManage.Instance.PushGameObject(showplant.name, showplant);
            showplant = null;
        }
        showplant = PoolManage.Instance.GetPoolGameObject("Plant", Attribute.Instance.GetPlantInfo(id).Prefab.name,PlantPos);//加载植物
        showplant.transform.localScale = Vector3.one * 100;
        showplant.GetComponent<Animator>().enabled = true;
        if (id % 100 == 4)
        {
            showplant.GetComponent<Animator>().SetTrigger("Build");
        }
        showplant.GetComponent<SortingGroup>().sortingLayerName = "Effect";
        PlantBase plantBase;
        if (showplant.TryGetComponent<PlantBase>(out plantBase))//清除植物代码
        {
            GameObject.Destroy(plantBase);
        }
        Attribute.PlantDescribe plantDescribe = Attribute.Instance.GetPlantInfo(id).plantDescribe;
        PlantName.text = plantDescribe.Name;
        PlantDescription.text = "<color=#0000FF><b>";
        PlantDescription.text += plantDescribe.MiniInfo;
        PlantDescription.text += "</b>\r\n \r\n<size=13>";
        PlantDescription.text += plantDescribe.KEYWORD;
        PlantDescription.text += "\r\n";
        PlantDescription.text += ProcessKeywords(plantDescribe.Characteristic);
        PlantDescription.text += "</size></color>\r\n \r\n";
        PlantDescription.text += plantDescribe.Description;
    }
    private string ProcessKeywords(string input)
    {
        // 转义关键词中的特殊字符（用于正则表达式）
        string pattern = "";
        foreach (string keyword in Attribute.Instance.buffdescribes.Keys)
        {
            if (string.IsNullOrEmpty(keyword)) continue;
            string escapedKeyword = Regex.Escape(keyword);
            pattern += $"{escapedKeyword}|";
        }
        pattern = pattern.TrimEnd('|');
        if (string.IsNullOrEmpty(pattern)) return input;

        // 使用正则表达式全局匹配并替换
        return Regex.Replace(
            input,
            pattern,
            match => $"{"<link><b><u>"}{match.Value}{"</b></u></link>"}",
            RegexOptions.IgnoreCase
        );
    }
    public void OnCilck(Attribute.DreamElement id)//点击元素卡片
    {
        BoxList.GetComponent<GridLayoutGroup>().enabled = false;
        if (nowlist.Contains(id))//添加或移除该元素
        {
            nowlist.Remove(id);
        }
        else
        {
            nowlist.Add(id);
        }
        for(int i = 0; i < cardlist.Count; i++)
        {
            if (Attribute.Instance.GetPlantInfo(cardlist[i].GetComponent<AlmanacsPlantBox>().id).DreamElement.Contains(DreamElement.Dream))
            {
                continue;
            }
            int newid = UImanage.GetElementPlant(nowlist, cardlist[i].GetComponent<AlmanacsPlantBox>().id);
            if (cardlist[i].GetComponent<AlmanacsPlantBox>().id != newid)
            {
                GameObject box = PoolManage.Instance.GetPoolGameObject("SeedCard", Attribute.Instance.GetPlantInfo(newid).CardPrefab.name, cardlist[i].transform.position, BoxList);
                RandomUtil.AddOrGetComponent<AlmanacsPlantBox>(box).Init(this, newid);
                PoolManage.Instance.PushGameObject(cardlist[i].name, cardlist[i]);
                cardlist[i] = box;
            }
        }
    }
}
