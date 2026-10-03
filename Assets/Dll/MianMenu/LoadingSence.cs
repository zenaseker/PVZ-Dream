using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSence : Singleton<LoadingSence>
{
    [SerializeField, Header("加载进度条")]
    private GameObject _loadingBarObject;
    [SerializeField, Header("加载进度条的图像")] 
    private Image _loadingBar;
    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>(); // 存储待加载场景的列表

    public override void Awake()
    {
        HideMenu();
    }

    //开始游戏

    public static void StartGame(string sencename)
    {
        LoadingSence.Instance._loadingBarObject.SetActive(true); // 启用加载进度条
        LoadingSence.Instance._scenesToLoad.Add(SceneManager.LoadSceneAsync(sencename, LoadSceneMode.Additive));
        LoadingSence.Instance.StartCoroutine(LoadingSence.Instance.ProgressLoadingBar()); // 启动异步加载进度条的协程
    }

    // 隐藏界面

    private void HideMenu()
    {
        _loadingBarObject.SetActive(false); // 禁用加载进度条
    }

    //异步加载进度条的协程

    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f; // 总的加载进度
        for (int i = 0; i < _scenesToLoad.Count; i++)
        {
            while (!_scenesToLoad[i].isDone)
            {
                loadProgress += _scenesToLoad[i].progress; // 累加加载进度
                _loadingBar.fillAmount = loadProgress / _scenesToLoad.Count; // 更新加载进度条的显示
                yield return null; // 等待下一帧
            }
        }
        HideMenu();
    }
}
