using DG.Tweening;
using System.Numerics;
using UnityEngine;

public class ZombieBody : MonoBehaviour
{
    public int X;
    public void Init(int x)
    {
        this.X = x;
    }
    public void Update()
    {
        this.GetComponent<Collider2D>().enabled = this.transform.position.y < MapManage.Instance.meshpos[X, 0].y;
    }
}
