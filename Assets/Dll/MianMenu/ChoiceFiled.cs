using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ChoiceFiled : MonoBehaviour
{
    public TextMeshProUGUI text;
    public List<string> filednames = new List<string>();
    public Transform Content;
    public GameObject button;
    public string FiledName = "";
    public void OnEnable()
    {
        filednames = new List<string>();
        DirectoryInfo VFX = new DirectoryInfo(SaveLoadManager.jsonFolder);
        if (VFX != null)
        {
            foreach (FileInfo fileInfo3 in VFX.GetFiles())
            {
                if (fileInfo3.Extension == ".sav")
                {
                    string name = Path.GetFileNameWithoutExtension(fileInfo3.Name);
                    filednames.Add(name);
                    GameObject button = GameObject.Instantiate(Content.GetChild(0).gameObject,Content);
                    button.name = name;
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                }
            }
            Content.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void OnChangeFiled(GameObject obj)
    {
        if (filednames.Contains(obj.name))
        {
            if (FiledName != obj.name)
            {
                FiledName = obj.name;
                button = obj;
            }
        }
    }
    public void OpenSave()
    {
        DebugShow.Instance.Init("存档位置在; " + SaveLoadManager.jsonFolder);
    }
    public void SetFiled()
    {
        Filed.Instance.defaultFiled.FiledName = FiledName;
        SaveLoadManager.Save<DefaultFiled>("Filed", Filed.Instance.defaultFiled, ".dfd");
        Filed.Instance.filedInfo = SaveLoadManager.Load<FiledInfo>(FiledName);
        Attribute.Instance.filedInfo = Filed.Instance.filedInfo;
        text.text = Filed.Instance.filedInfo.FliedName;
        this.gameObject.SetActive(false);
    }

    public void DelectFiled()
    {
        if (Filed.Instance.defaultFiled.FiledName == FiledName)
        {
            DebugShow.Instance.Init("禁止删除正在使用的存档！");
            return;
        }
        if (SaveLoadManager.IsExistsData(FiledName))
        {
            SaveLoadManager.Destory(FiledName);
            if (button != null)
            {
                GameObject.Destroy(button);
            }
        }
        else
        {
            Debug.Log("未找到存档");
        }
    }
}
