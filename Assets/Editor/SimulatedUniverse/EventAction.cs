using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAction
{
    public enum EventActionType
    {
        Close,//关闭
        DeBug,//报错
    }

    public void OnClick(EventActionType type)
    {
        switch (type)
        {
            case EventActionType.Close:
                Close();
                break;
            case EventActionType.DeBug:
                Debug();
                break;
        }
    }
    void Close()
    {
        UnityEngine.Debug.Log("事件已关闭");
    }
    void Debug()
    {
        UnityEngine.Debug.Log("这是一条报错");
    }
}
