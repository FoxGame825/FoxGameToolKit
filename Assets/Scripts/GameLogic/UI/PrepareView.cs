using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FoxGame.UI;
using FoxGame.Utils;
using FoxGame.Asset;

public class PrepareView : BaseView,IMsgReceiver
{
    public Text Content;
    public Image Img;
    public Button StartBtn;
    public GameObject MsgBox;
    public Button OkBtn;

    public override ViewConfigData ViewData {
        get {
            if (this._viewCfgData == null) {
                this._viewCfgData = new ViewConfigData();
                this._viewCfgData._viewID = ViewID.PrepareView;
                this._viewCfgData._isNavigation = true;
                this._viewCfgData._mountLayer = ViewMountLayer.Fixed;
                this._viewCfgData._layerSiblingIdx = 1;
            }
            return this._viewCfgData;
        }
    }

    private void Awake() {
        StartBtn.gameObject.SetActive(false);
        Img.gameObject.SetActive(false);
        MsgBox.SetActive(false);

        StartBtn.onClick.AddListener(()=> {
            GameMain.Instance.EnterGame();
        });

        OkBtn.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    void OnEnable() {
        MsgDispatcher.GetInstance().Subscribe(GameEvents.UpdateAsset.ShowUpdateAssetInfo, this);
        MsgDispatcher.GetInstance().Subscribe(GameEvents.UpdateAsset.UpdateAssetFinish, this);
    }

    void OnDisable() {
        MsgDispatcher.GetInstance().UnSubscribe(GameEvents.UpdateAsset.ShowUpdateAssetInfo, this);
        MsgDispatcher.GetInstance().UnSubscribe(GameEvents.UpdateAsset.UpdateAssetFinish, this);
    }

    public bool OnMsgHandler(string msgName, params object[] args) {
        switch (msgName) {
            case GameEvents.UpdateAsset.ShowUpdateAssetInfo: {
                    //Content.text = (string)args[0];
                }
                break;
            case GameEvents.UpdateAsset.UpdateAssetFinish: {
                    Content.text = "资源更新完成";
                    StartBtn.gameObject.SetActive(true);
                    Img.gameObject.SetActive(true);
                }
                break;
        }
        return true;
    }


}
