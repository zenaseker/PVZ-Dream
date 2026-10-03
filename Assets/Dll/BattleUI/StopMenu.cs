using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StopMenu : Singleton<StopMenu>
{
    protected override bool CanAlive
    {
        get
        {
            return true;
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.TimeStop();
        }
    }
    public void TimeStop()
    {
        if (BattleManage.Instance != null)
        {
            if (BattleManage.Instance.TimeStop == true)
            {
                this.GoBackToGame();
                return;
            }
            BattleManage.Instance.TimeStop = true;
            transform.Find("ReStart").GetChild(0).GetComponent<TextMeshProUGUI>().text = "重新开始";
        }
        else
        {
            transform.Find("ReStart").GetChild(0).GetComponent<TextMeshProUGUI>().text = "恢复默认";
        }
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
        MusicManage.Instance.PlayEffect("pause", 1);
        MusicManage.Instance.BGMCheck(false);
        transform.Find("BGMVaule").GetComponent<Slider>().value = Attribute.Instance.filedInfo.BGMValue;
        transform.Find("BattleVaule").GetComponent<Slider>().value = Attribute.Instance.filedInfo.battlemusicValue;
        transform.Find("GameSpeed").GetComponent<Slider>().value = Attribute.Instance.filedInfo.GameSpeed * 2;
    }
    public void GoBackToGame()
    {
        if (BattleManage.Instance != null)
        {
            BattleManage.Instance.TimeStop = false;
        }
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
        this.gameObject.SetActive(false);
        MusicManage.Instance.PlayEffect("buttonclick", 1);
        MusicManage.Instance.BGMCheck(true);
    }
    public void GoMianMenu()
    {
        if (Filed.Instance != null)
        {
            GoBackToGame();
            return;
        }
        MusicManage.Instance.PlayEffect("buttonclick", 1);
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        this.gameObject.SetActive(false);
        Attribute.ChangeScene("MianMenu");
        MusicManage.Instance.BGMCheck(true);
    }
    public void ReStart()
    {
        MusicManage.Instance.PlayEffect("buttonclick", 1);
        if (BattleManage.Instance != null)
        {
            BattleManage.Instance.ReStart();
        }
        else
        {
            Attribute.Instance.filedInfo.BGMValue = transform.Find("BGMVaule").GetComponent<Slider>().value = 1f;
            Attribute.Instance.filedInfo.battlemusicValue = transform.Find("BattleVaule").GetComponent<Slider>().value = 1f;
            Attribute.Instance.filedInfo.GameSpeed = transform.Find("GameSpeed").GetComponent<Slider>().value = 2f;
            MusicManage.Instance.ChangeSize(Attribute.Instance.filedInfo.BGMValue, Attribute.Instance.filedInfo.battlemusicValue);
        }
        this.gameObject.SetActive(false);
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        MusicManage.Instance.BGMCheck(true);
    }

    public void SetBGMValue(float value)
    {
        Attribute.Instance.filedInfo.BGMValue = value;
        MusicManage.Instance.ChangeSize(Attribute.Instance.filedInfo.BGMValue, Attribute.Instance.filedInfo.battlemusicValue);
    }
    public void SetBattleValue(float value)
    {
        Attribute.Instance.filedInfo.battlemusicValue = value;
        MusicManage.Instance.ChangeSize(Attribute.Instance.filedInfo.BGMValue, Attribute.Instance.filedInfo.battlemusicValue);
    }
    public void SetGameSpeed(float value)
    {
        Attribute.Instance.filedInfo.GameSpeed = value / 2;
    }
}
