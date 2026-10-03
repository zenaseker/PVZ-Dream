using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultySet : MonoBehaviour
{
    public TextMeshProUGUI iocn;
    public TextMeshProUGUI info;
    int dinum = 0;
    static Dictionary<int, string> text = new Dictionary<int, string>
    {
        {0,"无" },
        {1,"僵尸出怪率+20%，僵尸生命上限+20%" },
        {2,"僵尸出怪率+40%，僵尸生命上限+40%\r\n初始梦境深度为2" },
        {3,"僵尸出怪率+60%，僵尸生命上限+60%\r\n初始梦境深度为4\r\n灰烬效果不再触发" },
        {4,"僵尸出怪率+80%，僵尸生命上限+80%\r\n初始梦境深度为6\r\n灰烬效果不再触发\r\n植物卡槽数-1" },
        {5,"僵尸出怪率+100%，僵尸生命上限+100%\r\n初始梦境深度为10\r\n灰烬效果不再触发\r\n植物卡槽数-2" },
    };
    public static Dictionary<int, Color> color = new Dictionary<int, Color>
    {
        {0,Color.green },
        {1,new Color(1,0.5f,0,1) },//橙
        {2,Color.yellow },
        {3,new Color(0,1,1,1) },//青
        {4,Color.red },
        {5,new Color(1f,0,1f,1) },//紫
    };
    void OnEnable()
    {
        dinum = Attribute.Instance.filedInfo.Difficulty;
        iocn.text = "难度" + dinum.ToString();
        iocn.color = color[dinum];
        info.text = text[dinum];
        info.color = color[dinum];
    }
    public void SetDifficulty(int addnum)
    {
        if (addnum > 0 && dinum >= 5)
        {
            return;
        }
        if (addnum < 0 && dinum <= 0)
        {
            return;
        }
        dinum += addnum;
        Attribute.Instance.filedInfo.Difficulty = dinum;
        iocn.text = "难度" + dinum.ToString();
        iocn.color = color[dinum];
        info.text = text[dinum];
        info.color = color[dinum];
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
    }
}
