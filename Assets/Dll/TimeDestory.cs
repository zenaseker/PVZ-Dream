using UnityEngine;

public class TimeDestory : MonoBehaviour
{
    private float time = 10f;
    public bool destory = false;
    public bool Tonullparent = true;
    public void Init(float time,bool todestory = false,bool tonullparent = true)
    {
        this.time = time;
        destory = todestory;
        Tonullparent = tonullparent;
    }
    public void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            if (destory)
            {
                GameObject.Destroy(gameObject);
                return;
            }
            PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject, Tonullparent);
            time = 99f;
        }
    }

}
