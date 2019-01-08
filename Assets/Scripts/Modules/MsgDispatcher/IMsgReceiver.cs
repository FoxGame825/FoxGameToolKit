using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMsgReceiver  {

    bool OnMsgHandler(string msgName, params object[] args);

}
