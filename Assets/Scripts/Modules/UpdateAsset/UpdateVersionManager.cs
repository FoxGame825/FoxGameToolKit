using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxGame;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Events;


public class UpdateVersionManager:MonoSingleton<UpdateVersionManager>
{
    private System.Version curVersion;
    private System.Version onlineVersion;

    public bool IsNeedUpdate;

    public void CheckVersion(UnityAction<bool> onComplate = null) {
        IsNeedUpdate = false;
        StartCoroutine(progress(onComplate));
    }

    IEnumerator progress(UnityAction<bool> onComplate) {
        //拉取服务器版本
        MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, "检测版本号...");

        UnityWebRequest req = UnityWebRequest.Get(GameConfigs.ServerVersionUrl);
        yield return req.SendWebRequest();

        if(req.isHttpError || req.isNetworkError) {
            Debug.LogError(req.error);
            yield break;
        }

        onlineVersion = new System.Version(req.downloadHandler.text);
        curVersion = new System.Version( Application.version);

        if(onlineVersion != curVersion) {
            Debug.LogFormat("当前版本不是最新版本({0}),请及时更新到最新版本({1})",curVersion,onlineVersion);
            IsNeedUpdate = true;
        }

        Debug.Log("版本检测完成!");

        if (onComplate != null) {
            onComplate(IsNeedUpdate);
        }

        yield return null;
    }
}
