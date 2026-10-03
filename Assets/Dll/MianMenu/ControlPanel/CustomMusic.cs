using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CustomMusic : MonoBehaviour
{
    public void ChangeCustomMusic(string path)
    {
        try
        {
            StartCoroutine(GetMusic(path));
            DebugShow.Instance.Init("等待音频加载");
        }
        catch(Exception x)
        {
            Debug.Log("文件位置错误或文件不存在！\n" + x);
        }
    }
    public void CloseCustomMusic()
    {
        Debug.Log("Close");
        Attribute.Instance.custommusic = null;
        Attribute.Instance.filedInfo.CustomMusic = null;
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
        MusicManage.Instance.ChangeBGM("MainMenu");
    }
    IEnumerator GetMusic(string path)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                DebugShow.Instance.Init("文件位置错误或文件不存在！");
                yield break;
            }
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            if (clip != null)
            {
                Attribute.Instance.custommusic = clip;
                Attribute.Instance.filedInfo.CustomMusic = path;
                SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
            }
            yield return null;
            MusicManage.Instance.ChangeCustomBGM();
            DebugShow.Instance.Init("音频读取成功！");
        };
    }
}
