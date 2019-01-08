using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxGame;
using UnityEngine;
using System.Collections;
using FoxGame.Asset;
using FoxGame.UI;

public class GameMain : MonoSingleton<GameMain> 
{
    public void InitGame() {
        Debug.Log("初始化游戏...");
        StartCoroutine(_init());
    }

    IEnumerator _init() {


#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif


        AssetManager.Instance.InitMode(GameConfigs.LoadAssetMode);
        UIManager.Instance.OpenView(ViewID.PrepareView, null);
        yield return new WaitForEndOfFrame();

        Debug.Log("跳过版本检测...");
        //UpdateVersionManager.Instance.CheckVersion((bool needUpdate) => {
        //    if (needUpdate) {
        //        MsgBox.SetActive(true);
        //    } else {
        //        UpdateAssetManager.Instance.CheckAsset(() => {
        //            MsgDispatcher.GetInstance().Fire(GameEvents.Msg_DownloadFinish);
        //        });
        //    }
        //});


        // load and parse config data
        MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, "加载配置...");
        ConfigManager.Instance.ParseConfigData(AssetManager.Instance.LoadAsset<TextAsset>(GameConfigs.GetConfigPath("DataConfig")).bytes);

        MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.UpdateAssetFinish);


        yield return null;
    }


    public void EnterGame() {
        MsgDispatcher.GetInstance().Fire(GameEvents.GameProcess.EnterGame);
        UIManager.Instance.OpenView(ViewID.SignInView,OpenViewTag.HidePrevious,true,null);
    }

}
