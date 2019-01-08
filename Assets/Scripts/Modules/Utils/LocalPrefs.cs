using UnityEngine;
using System.Text;

/// <summary>
/// 本地数据
/// </summary>

namespace FoxGame.Utils {

    public class LocalPrefs {
        #region 账户信息
        private const string ACCOUNT = "ACCOUNT";
        private const string ACCOUNT_PASSWORD = "ACCOUNT_PSD";
        #endregion


        /// <summary>
        /// 前缀类型
        /// </summary>
        public enum PrefixType {
            /// <summary>
            /// 游戏前缀
            /// </summary>
            Game,
            /// <summary>
            /// 角色ID前缀
            /// </summary>
            RoleID
        }

        #region 账户信息
        /// <summary>
        /// 写入账户信息
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        public static void WriteAccountInfo(string account, string password) {
            PlayerPrefs.SetString(ACCOUNT, account);
            PlayerPrefs.SetString(ACCOUNT_PASSWORD, password);
        }

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public static void GetAccountInfo(out string account, out string password) {
            account = PlayerPrefs.GetString(ACCOUNT);
            password = PlayerPrefs.GetString(ACCOUNT_PASSWORD);
        }
        #endregion

        #region 写入数据
        public static void WriteValue(string key, int value, PrefixType prefixType) {
            key = GetKey(key, prefixType);
            PlayerPrefs.SetInt(key, value);
        }

        public static void WriteValue(string key, float value, PrefixType prefixType) {
            key = GetKey(key, prefixType);
            PlayerPrefs.SetFloat(key, value);
        }

        public static void WriteValue(string key, bool value, PrefixType prefixType) {
            key = GetKey(key, prefixType);

            int intValue = value ? 1 : 0;
            PlayerPrefs.SetInt(key, intValue);
        }

        public static void WriteValue(string key, string value, PrefixType prefixType) {
            key = GetKey(key, prefixType);
            PlayerPrefs.SetString(key, value);
        }
        #endregion

        #region 获取数据
        public static bool GetInt32Value(string key, out int value, PrefixType prefixType) {
            value = 0;
            key = GetKey(key, prefixType);

            if (PlayerPrefs.HasKey(key.ToString())) {
                value = PlayerPrefs.GetInt(key);
                return true;
            }

            return false;
        }

        public static bool GetSingleValue(string key, out float value, PrefixType prefixType) {
            value = 0.0f;
            key = GetKey(key, prefixType);

            if (PlayerPrefs.HasKey(key)) {
                value = PlayerPrefs.GetFloat(key);
                return true;
            }

            return false;
        }

        public static bool GetBoolValue(string key, out bool value, PrefixType prefixType) {
            value = false;
            key = GetKey(key, prefixType);

            if (PlayerPrefs.HasKey(key)) {
                value = PlayerPrefs.GetInt(key) == 1 ? true : false;
                return true;
            }

            return false;
        }

        public static bool GetStringValue(string key, out string value, PrefixType prefixType) {
            value = string.Empty;
            key = GetKey(key, prefixType);

            if (PlayerPrefs.HasKey(key)) {
                value = PlayerPrefs.GetString(key);
                return true;
            }

            return false;
        }
        #endregion

        private static string GetKey(string key, PrefixType prefixType) {
            string keyPrefix = "";
            if (prefixType == PrefixType.RoleID) {
                //keyPrefix = PlayerMod.Inst.RoleID.ToString();

                if (string.IsNullOrEmpty(keyPrefix)) {
                    Debug.LogError("未设置当前账户的角色ID");
                    return string.Empty;
                }
            } else {
                keyPrefix = "Game";
            }

            StringBuilder sb = new StringBuilder(keyPrefix);
            sb.Append("_");
            sb.Append(key);
            return sb.ToString();
        }
    }

}
