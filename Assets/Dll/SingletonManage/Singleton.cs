
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    protected virtual bool CanAlive
    { 
        get
        {
            return false;
        }
    }
    public static T Instance
    {
        get
        {
            if (instance == null && Attribute.Instance.InsetanceCanNew[typeof(T)])
            {
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + typeof(T).ToString()));
                obj.name = typeof(T).ToString();
                instance = obj.GetComponent<T>();
                obj.SetActive(true);
                GameObject.DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (CanAlive)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
