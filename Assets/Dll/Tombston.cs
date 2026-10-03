using UnityEngine;

public class Tombston : MonoBehaviour
{
    public Sprite[] Sprite1;
    public Sprite[] Sprite2;
    public Vector2Int pos;
    public void OnEnable()
    {
        int num = Random.Range(0, 19);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Sprite1[num];
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Sprite2[num];
        ZombieManage.Instance.BigFlag += InBigFlag;
    }
    public void InBigFlag(int flag)
    {
        int id = Random.Range(0, 2);
        GameObject obj = ZombieManage.Instance.InitZombie(id, pos.x);
        ZombieManage.Instance.LoadZombieMess(obj, id, pos.x);
        obj.transform.position = this.transform.position + Vector3.down * 0.5f;
        obj.GetComponent<ZombiesBase>().ZombieUp();
    }
    private void OnDestroy()
    {
        ZombieManage.Instance.BigFlag -= InBigFlag;
    }
}

