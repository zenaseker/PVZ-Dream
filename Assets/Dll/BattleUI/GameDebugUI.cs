using System;
using TMPro;

public class GameDebugUI:Singleton<GameDebugUI>
{
    public Action buttonbyleft;
    public Action buttonmiddle;
    public Action buttombyright;
    public TextMeshProUGUI _mian;
    public TextMeshProUGUI _left;
    public TextMeshProUGUI _middle;
    public TextMeshProUGUI _right;

    protected override bool CanAlive
    {
        get
        {
            return true;
        }
    }
    public void Init(string show,string left,string middle,string right,Action leftbutton,Action middlebutton,Action rightbutton)
    {
        gameObject.SetActive(true);
        _mian.text = show;
        if (leftbutton == null)
        {
            _left.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _left.text = left;
            _left.transform.parent.gameObject.SetActive(true);
            buttonbyleft = leftbutton;
        }
        if (middlebutton == null)
        {
            _middle.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _middle.text = middle;
            _middle.transform.parent.gameObject.SetActive(true);
            buttonmiddle = middlebutton;
        }
        if (rightbutton == null)
        {
            _right.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _right.text = right;
            _right.transform.parent.gameObject.SetActive(true);
            buttombyright = rightbutton;
        }
    }
    public void OnClickByLeft()
    {
        buttonbyleft?.Invoke();
        gameObject.SetActive(false);
    }
    public void OnClickByMiddle()
    {
        buttonmiddle?.Invoke();
        gameObject.SetActive(false);
    }
    public void OnClickByRight()
    {
        buttombyright?.Invoke();
        gameObject.SetActive(false);
    }
}
