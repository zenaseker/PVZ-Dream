using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlmanacsZombieBox : MonoBehaviour
{
    AlmanacsByZombie almanacsByZombie;
    int id;
    public void Init(AlmanacsByZombie almanacs,int ID)
    {
        almanacsByZombie = almanacs;
        id = ID;
        Sprite sprite = Attribute.GetSprite(Attribute.Instance.GetZombieInfo(ID).Prefab.name);
        this.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        this.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.rect.width / sprite.rect.height * 50, 50) * 4;
    }
    public void OnCilck()
    {
        almanacsByZombie.OnCilck(id);
    }
}
