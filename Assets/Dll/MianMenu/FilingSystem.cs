using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class FilingSystem : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject _Input;
    public GameObject _Choice;
    public string filedname = "";
    public void OnClick()
    {
//#if UNITY_EDITOR
        GameDebugUI.Instance.Init("这是你的存档么？", "新建存档", "选择存档", "返回游戏", LoadDebugConfirm, SelectFlie, GoBack);
//#else
//        GameDebugUI.Instance.Init("这是你的存档么？", "新建存档", "选择存档", "返回游戏", LoadDebugConfirm, null, GoBack);
//#endif
    }
    public void LoadDebugConfirm()//打开新建存档窗口
    {
        _Input.SetActive(true);
    }
    public void SelectYourFlie()//选取存档（il2cpp未生效）
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "sav files (*.sav)|*.sav";
        openFileDialog.InitialDirectory = SaveLoadManager.jsonFolder.Replace("/", "\\");
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string s = openFileDialog.FileName.Remove(0, SaveLoadManager.jsonFolder.Length);
            s = s.Remove(s.Length - 4, 4);
            Filed.Instance.defaultFiled.FiledName = s;
            SaveLoadManager.Save<DefaultFiled>("Filed", Filed.Instance.defaultFiled, ".dfd");
            Filed.Instance.filedInfo = SaveLoadManager.Load<FiledInfo>(s);
            Attribute.Instance.filedInfo = Filed.Instance.filedInfo;
            text.text = Filed.Instance.filedInfo.FliedName;
        }
    }
    public void SelectFlie()
    {
        _Choice.SetActive(true);
    }

    public void GoBack()//空白内容勿删
    {

    }
    public void OnFiledNameInput(string name)
    {
        filedname = name;
        text.text = name;
    }
    public void ChoiceNewFile()
    {
        FiledInfo filedInfo = new FiledInfo();
        filedInfo.Unclockplantid = new List<int> { 0 };
        filedInfo.FliedName = filedname;
        Attribute.Instance.filedInfo = filedInfo;
        Filed.Instance.defaultFiled.FiledName = filedname;
        SaveLoadManager.Save<DefaultFiled>("Filed", Filed.Instance.defaultFiled, ".dfd");
        SaveLoadManager.Save(filedInfo.FliedName, filedInfo);
        text.text = filedInfo.FliedName;
        _Input.SetActive(false);
    }
    public void Back()
    {
        text.text = Filed.Instance.defaultFiled.FiledName;
        _Input.SetActive(false);
    }
}
