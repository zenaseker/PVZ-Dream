using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTex : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioSource;
    public void StartBattleMusic()
    {
        MusicManage.Instance.PlayEffect("readysetplant", 1);
    }

    public void InFlagMusic()
    {
        MusicManage.Instance.PlayEffect("hugewave", 1);
    }

    public void InFinalMusic()
    {
        MusicManage.Instance.PlayEffect("finalwave", 1);
    }
    public void AnimactorEnd()
    {
        this.gameObject.SetActive(false);
    }
}
