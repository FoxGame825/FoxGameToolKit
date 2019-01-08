using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FoxGame.UI;
using UnityEngine.Events;
using FoxGame.Utils;

public class SignInView : BaseView {

    public InputField AccountInput;
    public InputField PasswdInput;
    public Button RegisterBtn;
    public Text Forget;
    public Button LoginInBtn;



    public override ViewConfigData ViewData {
        get {
            if(this._viewCfgData == null) {
                this._viewCfgData = new ViewConfigData();
                this._viewCfgData._viewID = ViewID.SignInView;
                this._viewCfgData._isNavigation = true;
                this._viewCfgData._mountLayer = ViewMountLayer.Fixed;
                this._viewCfgData._layerSiblingIdx = 1;
            }
            return this._viewCfgData;
        }
    }


    private void Awake() {
        this.RegisterBtn.onClick.AddListener(() => {

        });

        UIEvents.Listen(this.Forget.gameObject).onClick = (GameObject go) => {

        };

        this.LoginInBtn.onClick.AddListener(() => {

        });
    }


    protected override void OnShowViewBegin(params object[] args) {
        base.OnShowViewBegin(args);

    }

}
