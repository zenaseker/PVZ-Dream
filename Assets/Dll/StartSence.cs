using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSence : MonoBehaviour
{
    [SerializeField, Header("加载进度条")]
    private GameObject _loadingBarObject;
    [SerializeField, Header("加载进度条的图像")]
    private Image _loadingBar;
    [SerializeField, Header("密钥输入框")]
    private GameObject _loadingKeyInput;
    [SerializeField, Header("开始按钮")]
    private GameObject _loadingGo;
    AsyncOperation asyncOperation;
    bool IsWin = false;



    /// <summary>
    /// 密钥
    /// </summary>
    public const string Key = "BZMeiYingHunTong";
    private const string IV = "DaoGouSiGeMaXian";
    string _InputKey;

    public void Start()
    {
        if (!SaveLoadManager.IsExistsData("Key", ".mjb"))
        {
            CreateIV("Dream");
        }
        FinishRoad();
    }
    //加密
    public static string Encrypt(string plainText, string key = Key, string iv = IV)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    // 解密
    public static string Decrypt(string cipherText, string key = Key, string iv = IV)
    {
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(buffer))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    public static void CreateIV(string text)
    {
        string encrypted, iv;
        iv = Encrypt(text, Key).Substring(0, 16);
        encrypted = Encrypt(text, Key, iv);
        string text2 = $"{iv}:{encrypted}";
        SaveLoadManager.Save<string>("Key", text2, ".mjb");
    }
    private void FinishRoad()
    {
#if UNITY_EDITOR_WIN
        IsWin = true;
#else
        _loadingKeyInput.SetActive(true);
        _loadingGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "确认密钥";
#endif
    }
    public void InputKey(string text)
    {
        _InputKey = text;
    }
    public void OnButtonCilck()
    {
        string[] parts;
        parts = SaveLoadManager.Load<string>("Key", ".mjb").Split(':');
        string decrypted = Decrypt(parts[1], Key, parts[0]);
        if (!IsWin)
        {
            if (_InputKey == decrypted)
            {
                _loadingKeyInput.SetActive(false);
                asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                StartCoroutine(ProgressLoadingBar()); // 启动异步加载进度条的协程
            }
            return;
        }
        asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        StartCoroutine(ProgressLoadingBar()); // 启动异步加载进度条的协程
    }

    //异步加载进度条的协程
    private IEnumerator ProgressLoadingBar()
    {
        while (!asyncOperation.isDone)
        {
            asyncOperation.allowSceneActivation = false;
            _loadingBar.fillAmount = asyncOperation.progress; // 更新加载进度条的显示
            if (asyncOperation.progress >= 0.9f)
            {
                _loadingBar.fillAmount = 1f;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null; // 等待下一帧
        }
    }
}
