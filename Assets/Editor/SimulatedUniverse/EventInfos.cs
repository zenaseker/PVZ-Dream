using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventInfos : ScriptableObject
{
    [Header("事件列表")]
    [SerializeField]
    public List<EventInfo> EventInfoes = new List<EventInfo>();

    /// <summary>
    /// 游戏中转为字典方便查找
    /// </summary>
    [HideInInspector]
    public static Dictionary<int, EventInfo> EventInfoInGame;

    /// <summary>
    /// 在字典中查找事件
    /// </summary>
    /// <param name="key">事件ID</param>
    /// <returns>查找到的事件，没有则返回Null</returns>
    public EventInfo GetValue(int key)
    {
        if (EventInfoInGame == null)
        {
            InitializeDictionary();
        }
        if (EventInfoInGame.TryGetValue(key, out var value))
        {
            return value;
        }
        Debug.Log("未找到事件信息");
        return null;
    }
    /// <summary>
    /// 将SO中的内容转存到字典中
    /// </summary>
    public void InitializeDictionary()
    {
        EventInfoInGame = new Dictionary<int, EventInfo>();
        foreach (var entry in EventInfoes)
        {
            if (entry == null) continue;
            EventInfoInGame[entry.ID] = entry;
        }
    }


    /// <summary>
    /// 事件
    /// </summary>
    [System.Serializable]
    public class EventInfo
    {
        [Header("事件ID")]
        public int ID;

        [Header("事件名称")]
        public string Name;

        [Header("事件图")]
        public Sprite MainImage;

        [Header("事件阶段列表(默认从第一个开始)")]
        public Event[] Events;
        /// <summary>
        /// 事件详细内容
        /// </summary>
        [System.Serializable]
        public class Event
        {
            [Header("位置")]
            public int Index;

            [Header("文本内容")]
            [TextArea(0,10)]
            public string Text;

            [Header("选项列表(注意不要超过4个)")]
            public Option[] Options;
            /// <summary>
            /// 选项信息
            /// </summary>
            [System.Serializable]
            public class Option
            {
                [Header("选项标题")]
                public string Title;

                [Header("选项文本")]
                public string Info;

                [Header("跳转事件")]
                public int Goto;

                [Header("跳转到事件阶段/执行委托")]
                public bool ToNewEvents;

                [Header("委托")]
                public EventAction.EventActionType EventAction;
            }
        }
    }
}
