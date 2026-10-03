using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Filed : Singleton<Filed>
{
    private string path = "Filed";
    public FiledInfo filedInfo;
    public DefaultFiled defaultFiled;
    public FilingSystem filingSystem;
    public void Start()
    {
        LoadDefaultFlie();
        if (SaveLoadManager.IsExistsData(defaultFiled.FiledName))
        {
            LoadData();
            CustomLevel();
        }
        else
        {
            AddItem();
            SaveData();
        }
        MusicManage.Instance.ChangeSize(Attribute.Instance.filedInfo.BGMValue, Attribute.Instance.filedInfo.battlemusicValue);
        if (Attribute.Instance.filedInfo.CustomMusic != null && !MusicManage.Instance.cusstombgm)
        {
            try
            {
                StartCoroutine(GetMusic(Attribute.Instance.filedInfo.CustomMusic));
            }
            catch (Exception x)
            {
                UnityEngine.Debug.Log("文件位置错误或文件不存在！\n" + x);
            }
        }
        else
        {
            MusicManage.Instance.ChangeBGM("MainMenu");
        }
        MusicManage.Instance.PlayEffect("roll_in", 1f);
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
    }
    IEnumerator GetMusic(string path)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                yield break;
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            if (clip != null)
            {
                Attribute.Instance.custommusic = clip;
                Attribute.Instance.filedInfo.CustomMusic = path;
            }
            yield return null;
            MusicManage.Instance.ChangeCustomBGM();
        };
    }
    public void CustomLevel()
    {
        DirectoryInfo VFX = new DirectoryInfo(SaveLoadManager.jsonFolder);
        if (VFX != null)
        {
            foreach (FileInfo fileInfo3 in VFX.GetFiles())
            {
                if (fileInfo3.Extension == ".lev")
                {
                    Attribute.Level customlevel = SaveLoadManager.Load<Attribute.Level>(Path.GetFileNameWithoutExtension(fileInfo3.Name),".lev");
                    if (Attribute.Instance.Levels[LevelType.Challenge].Find(x => x.ID == customlevel.ID) != null)
                    {
                        return;
                    }
                    Attribute.Instance.Levels[LevelType.Challenge].Add(customlevel);
                    UnityEngine.Debug.Log("自定义存档加载：" + customlevel.Name);
                }
            }
        }
    }
    public void LoadDefaultFlie()
    {
        if (SaveLoadManager.IsExistsData(path,".dfd"))
        {
            defaultFiled = SaveLoadManager.Load<DefaultFiled>(path,".dfd");
        }
        else
        {
            defaultFiled = new DefaultFiled();
            SaveLoadManager.Save(path, defaultFiled, ".dfd");
        }
    }
    public void AddItem()
    {
        filedInfo = new FiledInfo();
        filedInfo.Unclockplantid = new List<int> { 0 };
        Attribute.Instance.filedInfo = filedInfo;
    }
    public void SaveData()
    {
        SaveLoadManager.Save(defaultFiled.FiledName, filedInfo);
    }
    public void LoadData()
    {
        try
        {
            filedInfo = SaveLoadManager.Load<FiledInfo>(defaultFiled.FiledName);
            Attribute.Instance.filedInfo = filedInfo;
            filingSystem.text.text = filedInfo.FliedName;
        }
        catch(Exception x)
        {
            GameDebugUI.Instance.Init("\b存档读取错误,报错内容：\r\n" + x.Message, "创建新存档","打开文件位置", "退出游戏", LoadDebugConfirm,OpenFileLocation, LoadDebugCancel);
        }
        DOTween.SetTweensCapacity(5000, 50);
    }
    public void ShowStopMenu()
    {
        StopMenu.Instance.TimeStop();
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }
    public void LoadDebugConfirm()
    {
        AddItem();
        SaveData();
    }
    public void OpenFileLocation()
    {
        string filePath = $"{SaveLoadManager.jsonFolder}{Attribute.Instance.filedInfo.FliedName}.sav";
        if (!Directory.Exists(filePath))
        {
            string s = SaveLoadManager.jsonFolder;
            s = s.Replace("/", "\\");
            Process.Start("explorer.exe", s);
        };
    }
    public void LoadDebugCancel()
    {
        QuitGame();
    }
    public void Help()
    {
        foreach (LevelType type in Attribute.Instance.Levels.Keys)
        {
            foreach (Attribute.Level level in Attribute.Instance.Levels[type])
            {
                Attribute.SaveFiled(level.ID, type);
            }
        }
        DebugShow.Instance.Init("一键通关完成！");
    }
}
