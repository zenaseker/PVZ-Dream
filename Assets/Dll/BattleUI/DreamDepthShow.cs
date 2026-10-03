using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamDepthShow : MonoBehaviour
{
    bool show = false;
    public void Update()
    {
        if (show && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            CheckShow();
        }
    }
    public void CheckShow()
    {
        if (BattleManage.Instance.LevelDreamDepth <= 0) return;
        show = !show;
        this.transform.GetChild(0).gameObject.SetActive(show);
        this.transform.GetChild(0).GetChild(0).gameObject.SetActive(BattleManage.Instance.LevelDreamDepth > 0);
        this.transform.GetChild(0).GetChild(1).gameObject.SetActive(BattleManage.Instance.LevelDreamDepth > 2);
        this.transform.GetChild(0).GetChild(2).gameObject.SetActive(ZombieManage.Instance.zombielist_Elite.Count > 0);
        this.transform.GetChild(0).GetChild(3).gameObject.SetActive(BattleManage.Instance.LevelDreamDepth > 15);
    }
}
