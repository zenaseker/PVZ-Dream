using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SimulatedUniverse
{
    public class SimulatedUniverse : MonoBehaviour
    {
        //SO
        public EventInfos SO;
        //SO调用
        public static EventInfos EventInfos;
        //当前事件
        EventInfos.EventInfo EventInfo;
        //当前事件阶段
        EventInfos.EventInfo.Event Event;
        //事件代码执行
        EventAction Action;
        //标题
        public TextMeshProUGUI Head;
        //文本框
        public TextMeshProUGUI Text;
        //图片
        public Image Image;
        //显示的文本.
        private string str = "";
        //显示的速度.
        public float speed = 1;
        //打字机运行事件
        float time = 0f;

        /// <summary>
        /// 单例模式
        /// </summary>
        private static SimulatedUniverse _instance;
        public static SimulatedUniverse instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            EventInfos = SO;
            EventInfos.InitializeDictionary();
            Action = new EventAction();
        }
        /// <summary>
        /// 测试用
        /// </summary>
        private void Start()
        {
            Init(0);
        }
        /// <summary>
        /// 加载(全局调用)
        /// </summary>
        /// <param name="id"></param>
        public static void Init(int id)
        {
            instance.gameObject.SetActive(true);
            instance.LoadInfo(id);
        }
        /// <summary>
        /// 加载(本地运行)
        /// </summary>
        /// <param name="id"></param>
        public void LoadInfo(int id)
        {
            //隐藏按钮
            this.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
            //加载信息
            EventInfo = EventInfos.GetValue(id);
            Head.text = EventInfo.Name;
            Image.sprite = EventInfo.MainImage;
            //读取第一个事件阶段
            LoadEvent(0);
        }
        void Update()
        {
            ///任意按钮跳过打字机
            if (Input.anyKeyDown)
            {
                time = str.Length;
            }
        }
        /// <summary>
        /// 按钮跳转
        /// </summary>
        /// <param name="index">/事件阶段</param>
        public void JumpTo(int index)
        {
            if (Event.Options[index].ToNewEvents)//事件
            {
                Action.OnClick(Event.Options[index].EventAction);
            }
            else//跳转
            {
                LoadEvent(Event.Options[index].Goto);
            }
        }
        /// <summary>
        /// 加载事件阶段
        /// </summary>
        /// <param name="index">阶段</param>
        void LoadEvent(int index)
        {
            this.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
            Event = EventInfo.Events[index];
            str = Event.Text;
            time = 0;
            StartShowText();
        }
        
        void ShowButton()
        {
            for(int i = 0;i < 4; i++)
            {
                if (i > Event.Options.Length - 1)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    this.transform.GetChild(2).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = Event.Options[i].Title;
                    this.transform.GetChild(2).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = Event.Options[i].Info;
                }
            }
        }
        
        public void Close()
        {
            instance.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示文字.
        /// </summary>
        public void StartShowText()
        {
            InvokeRepeating("ShowText", 0, Time.deltaTime);
        }
        /// <summary>
        /// 文本打字机.
        /// </summary>
        private void ShowText()
        {
            if (time < str.Length)
            {
                time += Time.deltaTime * speed;

                Text.text = str.Substring(0, (int)time);
            }
            else
            {
                Text.text = str;
                CancelInvoke();
                ShowButton();
            }
        }
    }
}
