using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;


namespace FoxGame.UI
{

    public abstract class BaseView : MonoBehaviour
    {
        /// <summary>
        /// 界面的基础配置数据
        /// </summary>
        protected ViewConfigData _viewCfgData;
        public abstract ViewConfigData ViewData {get;}
        public bool Visible {get { return this.gameObject.activeInHierarchy; }}

        /// <summary>
        /// 没有会自动添加
        /// </summary>
        protected CanvasGroup _viewCanavsGroup;
        
        /// <summary>
        /// 界面是否能操作
        /// </summary>
        public bool CanOperation {
            get { return this._viewCanavsGroup.interactable; }
        }


        /// <summary>
        /// 初始化,创建界面时调用
        /// </summary>
        public void Init() {
            this._viewCanavsGroup = GetComponent<CanvasGroup>();
            if(this._viewCanavsGroup == null) {
                this._viewCanavsGroup = this.gameObject.AddComponent<CanvasGroup>();
            }
            //this.Visible = false;
        }


        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="needAnim">是否需要打开动画,该字段优先级大于ViewConfigData._ShowAnimType字段</param>
        /// <param name="onComplate">完成回调</param>
        public void OnShowView(bool needAnim = true,UnityAction onComplate = null,params object[] args) {
            setViewCanOperation(false);

            OnShowViewBegin(args);

            if (!needAnim || this.ViewData._ShowAnimType == ShowAnimationType.Nune) {
                OnShowViewComplate();
                if (onComplate != null) onComplate();

            } else {
                switch (this.ViewData._ShowAnimType) {
                    case ShowAnimationType.Nune:
                        break;
                    case ShowAnimationType.FadeIn: {
                            this._viewCanavsGroup.alpha = 0;
                            this._viewCanavsGroup.DOFade(1, 0.3F).SetEase(Ease.InBack).OnComplete(() =>
                            {
                                OnShowViewAnimationComplate();
                                OnShowViewComplate();
                                if (onComplate != null) onComplate();
                            });
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="needAnim">是否需要关闭动画,优先级同OpenView</param>
        /// <param name="onComplate">完成回调</param>
        public void OnHideView(bool needAnim = true,UnityAction onComplate = null) {
            if(!needAnim || this.ViewData._HideAnimType == HideAnimationType.Nune) {
                OnHideViewComplate();
                if (onComplate != null) onComplate();

            } else {
                switch (this.ViewData._HideAnimType) {
                    case HideAnimationType.Nune:
                        break;
                    case HideAnimationType.FadeOut: {
                            //this._viewCanavsGroup.alpha = 1;
                            this._viewCanavsGroup.DOFade(0, 0.3F).SetEase(Ease.OutBack).OnComplete(() => {
                                OnHideViewAnimationComplate();
                                OnHideViewComplate();
                                if (onComplate != null) onComplate();
                            });
                        }
                        break;
                }
            }
            
        }

        /// <summary>
        /// 子类调用
        /// </summary>
        public void OnClickedClose() {
            UIManager.Instance.CloseView(this.ViewData._viewID);
        }

        /// <summary>
        /// 打开窗口开始 , 子类重写,根据数据更新窗口
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnShowViewBegin(params object[] args) {}

        /// <summary>
        /// 打开窗口完成
        /// </summary>
        protected virtual void OnShowViewComplate() {
            setViewCanOperation(true);
        }

        /// <summary>
        /// 关闭窗口完成
        /// </summary>
        protected virtual void OnHideViewComplate() {}

        /// <summary>
        /// 隐藏/关闭窗口动画完成回调 , [_HideAnimType = Nune 不会被调用]
        /// </summary>
        protected virtual void OnHideViewAnimationComplate() { }

        /// <summary>
        /// 打开窗口动画完成回调 , [_ShowAnimType = Nune 不会被调用]
        /// </summary>
        protected virtual void OnShowViewAnimationComplate() { }

        /// <summary>
        /// 设置界面能否操作
        /// </summary>
        /// <param name="op"></param>
        private void setViewCanOperation(bool op) {
            this._viewCanavsGroup.interactable = op;
        }

    }

}