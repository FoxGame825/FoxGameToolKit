using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FoxGame.UI
{

    public enum ViewID
    {
        Nune,   
        PrepareView,
        SignInView,
        TransformView,
    }

    public enum OpenViewTag
    {
        Nune, //do nothing
        HidePrevious,    //隐藏上一个窗口
        //CloseOther,
        //HideOther,
    }

    public enum ShowAnimationType
    {
        Nune,
        FadeIn,
    }

    public enum HideAnimationType {
        Nune,
        FadeOut,
    }

    //UI挂接的层级节点,显示优先级由上到下逐渐增大
    public enum ViewMountLayer
    {
        Fixed,  //固定层
        Popup,  //弹出层
        MessageBox, //消息框层
    }


    public static class UIDefine
    {

        public static Dictionary<ViewID, string> viewPaths = new Dictionary<ViewID, string>() {
            { ViewID.Nune,""},
            { ViewID.PrepareView,"PrepareView"},
            { ViewID.SignInView,"SignInView"}
        };

    }

}