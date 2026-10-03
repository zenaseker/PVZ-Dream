using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon_Square : MonoBehaviour
{
    public int Color = 0;
    public void Update()
    {
        if (this.transform.position.x <= -14)
        {
            PoolManage.Instance.PushGameObject(this.gameObject.name, this.gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionUpdate(collision);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        CollisionUpdate(collision);
    }
    void CollisionUpdate(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plant")
        {
            if (BattleControl_Chameleon.color != Color)
            {
                BattleControl_Chameleon.Lose();
            }
        }
    }
}
