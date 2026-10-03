using UnityEngine;

public class DreamLevelMpa : MonoBehaviour
{
    public DreamLevelBackHideGameObjects[] GameObjects;

    public void Init(int dreamElement)
    {
        this.gameObject.SetActive(true);
        foreach (DreamLevelBackHideGameObjects objs in GameObjects)
        {
            if (objs.dreamElement == (Attribute.DreamElement)dreamElement)
            {
                Open(objs);
                return;
            }
        }
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
        foreach (DreamLevelBackHideGameObjects objs in GameObjects)
        {
            Close(objs);
        }
    }
    void Close(DreamLevelBackHideGameObjects dreamLevelBackHideGameObjects)
    {
        dreamLevelBackHideGameObjects.Parent.SetActive(true);

        foreach (DreamMianLevelInforation level in dreamLevelBackHideGameObjects.GameObjects)
        {
            level.gameObject.SetActive(false);
            if (level.line != null && level.line.Length > 0)
            {
                foreach (GameObject gameObject in level.line)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    void Open(DreamLevelBackHideGameObjects dreamLevelBackHideGameObjects)
    {
        dreamLevelBackHideGameObjects.Parent.SetActive(true);
        foreach (DreamMianLevelInforation level in dreamLevelBackHideGameObjects.GameObjects)
        {
            if (level.Uncolckuse != null && level.Uncolckuse.Length > 0)
            {
                foreach (int i in level.Uncolckuse)
                {
                    if (!Attribute.Instance.filedInfo.DreamFinishLevel.Contains(i))
                    {
                        continue;
                    }
                }
            }
            level.gameObject.SetActive(true);
            if (level.line != null && level.line.Length > 0)
            {
                foreach (GameObject gameObject in level.line)
                {
                    gameObject.SetActive(true);
                }
            }
        }
    }
}

[System.Serializable]
public class DreamLevelBackHideGameObjects
{
    public Attribute.DreamElement dreamElement;
    public GameObject Parent;
    public DreamMianLevelInforation[] GameObjects;
}