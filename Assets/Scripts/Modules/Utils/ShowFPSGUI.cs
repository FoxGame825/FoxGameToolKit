using UnityEngine;
using System.Collections;

/// <summary>
/// 帧数显示工具
/// </summary>

namespace FoxGame.Utils {

    public class ShowFPSGUI : MonoBehaviour {
        /// <summary>
        /// 更新周期
        /// </summary>
        public float UpdateInterval = 0.5F;

        /// <summary>
        /// 显示字体大小
        /// </summary>
        public int FontSize = 30;

        /// <summary>
        /// 每个周期的帧数积累
        /// </summary>
        private float _accum = 0;

        /// <summary>
        /// 每个周期的帧数
        /// </summary>
        private int _frames = 0;

        /// <summary>
        /// 每个周期的剩余时间
        /// </summary>
        private float _timeleft;

        private GUIStyle _fpsGuiStyle;
        private Color _fpsColor = Color.white;
        private string _fpsFormart = string.Empty;

        private void Start() {
            _timeleft = UpdateInterval;
        }

        private void Update() {
            _timeleft -= Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frames++;

            if (_timeleft <= 0.0f) {
                float fps = _accum / _frames;
                _fpsFormart = System.String.Format("{0:F2} FPS", fps);

                if (fps < 10.0f) {
                    _fpsColor = Color.red;
                } else {
                    if (fps < 30.0f) {
                        _fpsColor = Color.yellow;
                    } else {
                        _fpsColor = Color.green;
                    }
                }

                _timeleft = UpdateInterval;
                _accum = 0.0f;
                _frames = 0;
            }
        }

        private void OnGUI() {
            if (_fpsGuiStyle == null) {
                _fpsGuiStyle = new GUIStyle();
                _fpsGuiStyle.alignment = TextAnchor.UpperLeft;
                _fpsGuiStyle.fontSize = FontSize;
            }

            if (!string.IsNullOrEmpty(_fpsFormart)) {
                _fpsGuiStyle.normal.textColor = _fpsColor;

                int w = Screen.width;
                int h = Screen.height;
                Rect rect = new Rect(0, 0, w, h * 2 / 100);
                GUI.Label(rect, _fpsFormart, _fpsGuiStyle);
            }
        }
    }
}
