using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManage : Singleton<MusicManage>
{
    float _BGMsize = 1.0f;
    float _Effectsize = 1.0f;
    public bool cusstombgm = false;
    public AudioSource music1;
    public AudioSource music2;
    Dictionary<string, GameObject> Sourse = new Dictionary<string, GameObject>();
    protected override bool CanAlive 
    {
        get
        {
            return true;
        }
    }
    public void ChangeSize(float BGMsize,float Effectsize)
    {
        _BGMsize = BGMsize;
        _Effectsize = Effectsize;
        music1.volume = _BGMsize;
        music2.volume = _Effectsize;
    }
    public void ChangeCustomBGM()
    {
        music1.clip = Attribute.Instance.custommusic;
        music1.Play();
        cusstombgm = true;
    }
    public void ChangeBGM(string BGM)
    {
        music1.clip = Attribute.GetMusic(BGM);
        music1.Play();
    }
    public void PlayEffect(string effect,float size)
    {
        if (Sourse.TryGetValue(effect,out GameObject value))
        {
            if (value.activeSelf)
            {
                return;
            }
            value.SetActive(true);
            RandomUtil.AddOrGetComponent<TimeDestory>(value).Init(value.GetComponent<AudioSource>().clip.length, false, false);
            return;
        }
        GameObject effectsourse = PoolManage.Instance.GetPoolGameObject("Singleton", "EffectSourse", this.gameObject.transform);
        effectsourse.name = effect;
        effectsourse.GetComponent<AudioSource>().clip = Attribute.GetMusic(effect);
        effectsourse.GetComponent<AudioSource>().Play();
        effectsourse.GetComponent<AudioSource>().volume = size * Attribute.Instance.filedInfo.BGMValue;
        RandomUtil.AddOrGetComponent<TimeDestory>(effectsourse).Init(effectsourse.GetComponent<AudioSource>().clip.length,false,false);
        Sourse.Add(effect, effectsourse);
    }
    public void BGMCheck(bool flag)
    {
        if (flag)
        {
            music1.Play();
        }
        else
        {
            music1.Pause();
        }
    }
}
