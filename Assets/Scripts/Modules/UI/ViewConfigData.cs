using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FoxGame.UI
{
    public class ViewConfigData
    {
        public ViewID _viewID;
        public bool _isNavigation;  //是否加入导航功能
        public ShowAnimationType _ShowAnimType =  ShowAnimationType.FadeIn;
        public HideAnimationType _HideAnimType = HideAnimationType.FadeOut;
        public ViewMountLayer _mountLayer;  //挂接的层级
        public int _layerSiblingIdx;    //在该层级的显示优先级 [实际上,ugui内部的sibling索引不会超过子节点的数量, 因此使用ugui获取到的sibling索引与该字段可能不会相同,该字段会被用于uimanager的排序计算中]
    }
}