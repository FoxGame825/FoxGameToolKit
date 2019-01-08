using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgDispatcher {

    private Dictionary<string, List<IMsgReceiver>> m_Msgs = new Dictionary<string, List<IMsgReceiver>>();

    private static MsgDispatcher m_Ins;
    public static MsgDispatcher GetInstance() {
        if (m_Ins == null) {
            m_Ins = new MsgDispatcher();
        }
        return m_Ins;
    }


    //订阅消息
    public void Subscribe(string msg, IMsgReceiver recevier) {
        if (recevier == null) {
            Debug.LogError("SubscribeMsg : recevier == null");
            return;
        }

        if (Check(msg, recevier)) {
            Debug.LogFormat("SubscribeMsg: recevier has been subscribed ,msg={0},recevier={1}", msg, recevier.ToString());
        } else {
            if (this.m_Msgs.ContainsKey(msg)) {
                this.m_Msgs[msg].Add(recevier);
            } else {
                List<IMsgReceiver> list = new List<IMsgReceiver>();
                list.Add(recevier);
                this.m_Msgs.Add(msg, list);
            }
        }
    }

    // 取消订阅消息
    public void UnSubscribe(string msg, IMsgReceiver recevier) {
        if (recevier == null) {
            Debug.LogError("UnSubscribeMsg: recevier == null");
            return;
        }

        if (Check(msg, recevier)) {
            this.m_Msgs[msg].Remove(recevier);
        }
    }

    public void UnSubscribe(IMsgReceiver recevier) {
        if (recevier == null) {
            Debug.LogError("UnSubscribeMsg: recevier == null");
            return;
        }
        foreach (var iter in m_Msgs) {
            iter.Value.Remove(recevier);
        }
    }

    //检查订阅
    public bool Check(string msg, IMsgReceiver recevier) {
        if (m_Msgs.ContainsKey(msg)) {
            var list = m_Msgs[msg];
            return list.Contains(recevier);
        }
        return false;
    }

    //清除
    public void ClearAll() {
        m_Msgs.Clear();
    }

    //抛出消息
    public void Fire(string msg, params object[] args) {
        if (!this.m_Msgs.ContainsKey(msg)) {
            Debug.LogWarning("Fire msg: msg has no receiver!");
            return;
        }

        Debug.Log("[MsgDispatcher] fire msg:" + msg);

        List<IMsgReceiver> list = this.m_Msgs[msg];
        try {
            for (int i = 0; i < list.Count; ++i) {
                if (list[i] != null) {
                    bool bNext = list[i].OnMsgHandler(msg, args);
                    if (bNext) //有返回true的 消息会被截断,下面的handler不会接受到该消息
                        {
                        Debug.LogWarningFormat("Fire msg: msg[{0}] has been stop fire!", msg);
                        break;
                    }
                }
            }
        } catch {

        }
    }


}
