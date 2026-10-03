using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class PropBase : MonoBehaviour
{
    public GameObject prop;
    public PlantBase inplant = null;


    public virtual void Update()
    {
        bool hasplant = false;
        RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero , 1 << LayerMask.GetMask("Unit"));
        foreach (RaycastHit2D ray in raycastHit2D)
        {
            if (ray.collider != null && ray.transform.tag == "Plant")
            {
                hasplant = true;
                GetRayOnPlant(ray);
                break;
            }
        }
        if (!hasplant && inplant != null)
        {
            inplant.HighLighttime = 0f;
            inplant.ChangeLight(1f);
            inplant = null;
        }
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector3.z = 0;
        this.transform.position = vector3;
        if (Input.GetMouseButtonUp(1))
        {
            PropManage.Instance.CancelProp();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnClick();
        }
    }

    public virtual void GetRayOnPlant(RaycastHit2D raycastHit)
    {
        if (inplant != null)
        {
            if (raycastHit.transform.GetComponent<PlantBase>() == inplant)
            {
                return;
            }
            inplant.HighLighttime = 0f;
            inplant.ChangeLight(1f);
            inplant = null;
        }
        inplant = raycastHit.transform.GetComponent<PlantBase>();
        inplant.HighLighttime = 99f;
        inplant.ChangeLight(1.3f);
    }

    public virtual void OnClick()
    {
        if (HandManage.Instance.hand != null)
        {
            HandManage.Instance.ClearHandPlant();
        }
    }

    public void ShowProp()
    {
        prop.SetActive(true);
        OnShow();
    }

    public void HideProp()
    {
        if (inplant != null)
        {
            inplant.HighLighttime = 0f;
            inplant.ChangeLight(1f);
        }
        prop.SetActive(false);
        OnHide();
    }

    public virtual void OnShow()
    {

    }
    public virtual void OnHide()
    {

    }
}
