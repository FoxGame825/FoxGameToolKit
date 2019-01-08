using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using AssetManager = FoxGame.Asset.AssetManager;


namespace FoxGame.UI
{

    public class UIManager : MonoSingleton<UIManager>, IUIManager
    {
        private UISetting _uiSetting;
        private Dictionary<ViewID, BaseView> _viewDic = new Dictionary<ViewID, BaseView>();
        private Stack<ViewNavigationData> _navigationStack = new Stack<ViewNavigationData>();


        public BaseView CurView {
            get {
                BaseView view = null;
                if (this._navigationStack.Count > 0) {
                    var pre = this._navigationStack.Peek();
                    this._viewDic.TryGetValue(pre.viewID, out view);
                }
                return view;
            }
        }

        public Transform UIRoot {get { return _uiSetting.UIRoot; }}
        public Camera UICamera { get { return _uiSetting.UICamera; } }
        public RectTransform FixedLayer { get { return _uiSetting.FixedLayer; } }
        public RectTransform PopupLayer { get { return _uiSetting.PopupLayer; } }
        public RectTransform MessageBoxLayer { get { return _uiSetting.MessageBoxLayer; } }

        
        public override void Init() {
            base.Init();
            //加载uiRootprefab , 加载依赖AssetManager,需要保证AssetManager已经完成了初始化
            var uiSetting_prefab = AssetManager.Instance.LoadAsset<UISetting>(GameConfigs.GetUIPath("UIRoot"));

            _uiSetting = GameObject.Instantiate<UISetting>(uiSetting_prefab, this.transform);
            Debug.Assert(_uiSetting != null, "初始化UI管理器失败!");

        }

        public void OpenView(ViewID viewID, UnityAction onComplate = null,params object[] args) {
            this.OpenView(viewID, OpenViewTag.Nune, true, onComplate, args);
        }

        public void OpenView(ViewID viewID, OpenViewTag openTag = OpenViewTag.Nune, bool needAnim = true, UnityAction onComplate = null, params object[] args) {

            BaseView view = null;
            this._viewDic.TryGetValue(viewID, out view);
            if (view != null) {
                Debug.Log("[UIManager] OpenView: " + viewID);

                ViewConfigData viewCfgData = view.ViewData;

                switch (openTag) {
                    case OpenViewTag.HidePrevious: {
                            if (this._navigationStack.Count > 0) {
                                CloseView(this._navigationStack.Peek().viewID, false, false, null);
                            }
                        }
                        break;
                }

                if (viewCfgData._isNavigation) {
                    pushToNavigation(new ViewNavigationData() { viewID = viewCfgData._viewID, data = args });
                }

                switch (viewCfgData._mountLayer) {
                    case ViewMountLayer.Fixed: {
                            refreshSibling(view, this.FixedLayer);
                        }
                        break;
                    case ViewMountLayer.Popup: {
                            refreshSibling(view, this.PopupLayer);
                        }
                        break;
                    case ViewMountLayer.MessageBox: {
                            refreshSibling(view, this.MessageBoxLayer);
                        }
                        break;
                }


                view.gameObject.SetActive(true);
                view.OnShowView(needAnim, onComplate, args);

            } else {
                Debug.Log("[UIManager] loadView: " + viewID);
                AssetManager.Instance.LoadAssetAsync<BaseView>(GameConfigs.GetUIPath(UIDefine.viewPaths[viewID]), (BaseView viewPrefab) => {
                    view = GameObject.Instantiate(viewPrefab).GetComponent<BaseView>();

                    switch (view.ViewData._mountLayer) {
                        case ViewMountLayer.Fixed: {
                                view.transform.SetParent(this.FixedLayer);
                            }
                            break;
                        case ViewMountLayer.Popup: {
                                view.transform.SetParent(this.PopupLayer);
                            }
                            break;
                        case ViewMountLayer.MessageBox: {
                                view.transform.SetParent(this.MessageBoxLayer);
                            }
                            break;
                    }

                    RectTransform rt = view.GetComponent<RectTransform>();
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.one;

                    view.transform.localScale = Vector3.one;
                    view.transform.localPosition = Vector3.zero;
                    view.gameObject.name = viewID.ToString();
                    GameUtils.SetLayer(view.gameObject, LayerMask.NameToLayer("UI"));
                    view.Init();

                    this._viewDic.Add(viewID, view);

                    OpenView(viewID, openTag, needAnim, onComplate,args);
                });
            }

        }


        public void CloseView(ViewID viewID, bool needAnim = true, bool needDestroy = false, UnityAction onComplate = null) {
            Debug.Log("[UIManager]CloseView :" + viewID);

            BaseView view = null;
            this._viewDic.TryGetValue(viewID, out view);
            if (view != null) {
                view.OnHideView(needAnim,() => {
                    view.gameObject.SetActive(false);

                    if (needDestroy) {
                        this._viewDic.Remove(viewID);
                        DestroyObject(view.gameObject);
                    }

                    if (onComplate != null) onComplate();
                });
            }
        }


        
        private void refreshSibling(BaseView view,RectTransform layer) {
            view.transform.SetAsLastSibling();
            for(int i = 0; i < layer.childCount; ++i) {
                Transform ts = layer.GetChild(i);
                var childView = ts.GetComponent<BaseView>();
                if(childView!=null && childView !=view && view.ViewData._layerSiblingIdx < childView.ViewData._layerSiblingIdx) {
                    view.transform.SetSiblingIndex(childView.transform.GetSiblingIndex());
                    return;
                }
            }
        }



        #region Navigation 
        public void NavigationClear() {
            this._navigationStack.Clear();
        }

        public bool NavigationPreviousView() {
            Debug.Log("导航到上一个窗口");

            if (this._navigationStack.Count > 1) {
                var closeView = this._navigationStack.Pop();
                var preView = this._navigationStack.Peek();

                CloseView(closeView.viewID);

                if(preView!=null && this._viewDic.ContainsKey(preView.viewID) && !this._viewDic[preView.viewID].Visible) {
                    //导航栈中删除
                    this._navigationStack.Pop();
                    OpenView(preView.viewID,null);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 导航栈恢复
        /// </summary>
        public void NavigationRestore() {
            var ar = this._navigationStack.ToArray();
            NavigationClear();
            for (int i = 0; i < ar.Length; ++i) {
                OpenView(ar[i].viewID, OpenViewTag.Nune, false, null, ar[i].data);
            }
        }

        void pushToNavigation(ViewNavigationData data) {
            if (this._navigationStack.Count > 0) {
                var previous = this._navigationStack.Peek();
                if(previous !=null && previous.viewID == data.viewID) {
                    return;
                }
            }
            this._navigationStack.Push(data);
        }

        #endregion

    }

}

