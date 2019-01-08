using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 长按按钮
/// </summary>

namespace FoxGame.UI {

    public class LongPressButton : Button {
        private enum PressType {
            Unknow,
            /// <summary>
            /// 按钮按下
            /// </summary>
            PressDown,
            /// <summary>
            /// 按钮长按
            /// </summary>
            LongPress
        }

        private ButtonClickedEvent _longPressDown = new ButtonClickedEvent();
        private ButtonClickedEvent _longPressUp = new ButtonClickedEvent();
        private ButtonClickedEvent _pressClick = new ButtonClickedEvent();

        private PressType _pressType = PressType.Unknow;

        /// <summary>
        /// 手指按下经过的时间
        /// </summary>
        private float _pressDownRealStartTime;

        #region 属性
        public ButtonClickedEvent OnLongPressDown { get { return _longPressDown; } }
        public ButtonClickedEvent OnLongPressUp { get { return _longPressUp; } }
        public ButtonClickedEvent OnPressClick { get { return _pressClick; } }
        #endregion

        protected LongPressButton() {
        }

        private void Update() {
            if (_pressType == PressType.PressDown) {
                if (Time.realtimeSinceStartup - _pressDownRealStartTime >= 0.2) {
                    _pressType = PressType.LongPress;
                    _longPressDown.Invoke();
                }
            }
        }

        public override void OnPointerDown(PointerEventData eventData) {
            if (!interactable) {
                return;
            }

            base.OnPointerDown(eventData);

            _pressDownRealStartTime = Time.realtimeSinceStartup;
            _pressType = PressType.PressDown;
        }

        public override void OnPointerUp(PointerEventData eventData) {
            if (!interactable) {
                return;
            }

            base.OnPointerUp(eventData);

            switch (_pressType) {
                case PressType.LongPress:
                    _longPressUp.Invoke();
                    break;

                case PressType.PressDown:
                    _pressClick.Invoke();
                    break;
            }

            _pressType = PressType.Unknow;
        }
    }
}

