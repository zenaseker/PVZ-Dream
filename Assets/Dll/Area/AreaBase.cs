using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlinkArea;

public class AreaBase : MonoBehaviour
{
    public int line = -1;
    public virtual void Init(int line)
    {
        this.line = line;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie" || collision.gameObject.tag == "Plant")
        {
            InArea(collision.gameObject.GetComponent<BattleUnitModel>());
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Zombie" || collision.gameObject.tag == "Plant")
        {
            OutArea(collision.gameObject.GetComponent<BattleUnitModel>());
        }
    }
    public virtual void InArea(BattleUnitModel battleUnitModel)
    { 
    }
    public virtual void OutArea(BattleUnitModel battleUnitModel)
    {
    }
}
