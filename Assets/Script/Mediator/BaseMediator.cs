using System;
using System.Collections.Generic;

namespace AIExhibition.Mediator
{
    public delegate void CallBack();
    public delegate void CallBack<in T>(T arg);

    public class BaseMediator
    {
        public static BaseMediator Instance { get; private set; } = new BaseMediator();
        
        //儲存所有事件的字典，key為事件名稱，value為對應的委托，因此這個中介者是使用string查找事件來調用實現的。
        private Dictionary<string, Delegate> _messages;
        public Dictionary<string, Delegate> Messages {
            get {
                if (_messages == null) _messages = new Dictionary<string, Delegate>();
                return _messages;
            }
            private set => _messages = value;
        }
        
        public void Broadcast<T>(string eventName, T arg0)
        {
            if (Messages.TryGetValue(eventName, out Delegate d)) 
            {
                //檢查事件是否為同參數，如果是，調用他。
                if (d is CallBack<T> callBack)
                    callBack(arg0);
                else
                    throw new Exception(string.Format("廣播事件錯誤：事件{0}對應委托有不同的類型", eventName));
                //BUG 如果找到的事件已經空了，他會丟出例外。
            }
        }
        public void Broadcast(string eventName)
        {
            if (Messages.TryGetValue(eventName, out Delegate d)) 
            {
                //檢查事件是否為同參數，如果是，調用他。
                if (d is CallBack callBack)
                    callBack();
                else if (d == null) {}
                else
                    throw new Exception(string.Format("廣播事件錯誤：事件{0}對應委托有不同的類型", eventName));
            }
        }

        public void AddListener<T>(string eventName, CallBack<T> callBack)
        {
            //檢查是否包含同名事件，如果包含且參數不等，拋出錯誤。
            CheckIsContainSameNameEvent(eventName, callBack);
            //嘗試增加，如果有同名事件，則不增加，如果沒有同名事件，則增加。
            Messages.TryAdd(eventName, null);
            Messages[eventName] = (CallBack<T>)Messages[eventName] + callBack;
        }
        public void AddListener(string eventName, CallBack callBack)
        {
            //檢查是否包含同名事件，如果包含且參數不等，拋出錯誤。
            CheckIsContainSameNameEvent(eventName, callBack);
            //嘗試增加，如果有同名事件，則不增加，如果沒有同名事件，則增加。
            Messages.TryAdd(eventName, null);
            Messages[eventName] = (CallBack)Messages[eventName] + callBack;
        }

        public void RemoveListener<T>(string eventName, CallBack<T> callBack)
        {
            //檢查事件狀態，包刮是否存在事件，是否事件含有委託，是否與事件委託參數一致。
            CheckEventState(eventName, callBack);
            //移除委託。
            Messages[eventName] = (CallBack<T>)Messages[eventName] - callBack;
        }
        public void RemoveListener(string eventName, CallBack callBack)
        {
            //檢查事件狀態，包刮是否存在事件，是否事件含有委託，是否與事件委託參數一致。
            CheckEventState(eventName, callBack);
            //移除委託。
            Messages[eventName] = (CallBack)Messages[eventName] - callBack;
        }
        
        private void CheckEventState(string eventName, Delegate callBack)
        {

            if (Messages.TryGetValue(eventName, out var d)) 
            {
                if (d == null) 
                {
                    throw new Exception(string.Format("移除監聽事件錯誤：事件{0}沒有對應的委托", eventName));
                } 
                else if (d.GetType() != callBack.GetType()) 
                {
                    throw new Exception(string.Format("移除監聽事件錯誤：嘗試為事件{0}移除不同類型的委托" +
                                                      "，當前事件所對應的委托為{1}，要移除的委托是{2}"
                                                        , eventName, d.GetType(), callBack.GetType()));
                }
            } 
            else 
            {
                throw new Exception(string.Format("移除監聽事件錯誤：沒有事件碼{0}", eventName));
            }
        }
        private void CheckIsContainSameNameEvent(string eventName,Delegate callback) {
            //如果包含的話，則判斷委託是否為null，如果不為null，則說明已經有委託註冊進去了，這時候就要判斷委託的類型是否一致，如果不一致，代表已經有同名事件但是參數類型不一樣;
            if (Messages.TryGetValue(eventName, out Delegate d))
            {
                if (d != null && d.GetType() != callback.GetType()) 
                {
                    throw new Exception(string.Format("嘗試為事件{0}添加不同類型的委托，當前事件所對應的委托是{1}，" +
                                                      "要添加的委托是{2}", eventName, d.GetType(), callback.GetType()));
                }
            }
                 
        }
    }
}