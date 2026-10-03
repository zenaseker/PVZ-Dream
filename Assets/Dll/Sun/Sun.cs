using UnityEngine;
using DG.Tweening;

public class Sun : MonoBehaviour
{
    Vector3 vector3 = new Vector3(-10.6f, 5.18f, -5f);
    public int num = 25;
    public void ChangeNum(int num)
    {
        this.num = num;
        this.transform.localScale = Vector3.one * 1.5f * (num < 5 ? 5 :num) / 25;
    }
    public void CheckSun()
    {
        MusicManage.Instance.PlayEffect("points", 1);
        transform.DOPath(new Vector3[] { transform.position, vector3 }, 0.3f).SetEase(Ease.Linear).onComplete += GetSun;
    }
    public void GetSun()
    {
        PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
        BattleManage.Instance.SunnumberChange(num);
    }
}
