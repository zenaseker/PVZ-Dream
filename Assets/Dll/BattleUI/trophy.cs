using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class trophy : MonoBehaviour
{
    bool select = false;
    public Transform Trophy;
    public void OnEnable()
    {
        if (BattleManage.Instance.level?.unclockplantid?[0] != 0)
        {
            GameObject obj = GameObject.Instantiate(Attribute.Instance.GetPlantInfo(BattleManage.Instance.level.unclockplantid[0]).CardPrefab,Trophy);
            obj.transform.localScale = Vector3.one * 0.006f;
            obj.GetComponent<RectTransform>().anchorMax = obj.GetComponent<RectTransform>().anchorMin = Vector2.one / 2;
            GameObject.Destroy(obj.GetComponent<Button>());
            Trophy.GetComponent<Image>().enabled = false;
        }
        ObjectJump objectJump = this.gameObject.AddComponent<ObjectJump>();
        objectJump.Move(this.transform.position + new Vector3(this.transform.position.x < 0 ? 1f : -1f, -1f, 0));
    }
    public void PlayMusic()
    {
        if (select) return;
        MusicManage.Instance.ChangeBGM("winmusic");
        select = true;
        this.transform.DOPath(new Vector3[] { this.transform.position, Vector3.zero }, 1f, PathType.Linear);
    }
    public void GotoMianMenu()
    {
        BattleManage.Instance.ReturnMianMenu(true);
    }
}
