using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// string解析(用于配置文件字符串解析)
/// </summary>

namespace FoxGame.Utils {

    public struct Param1 {
        public int Value1;

        public Param1(int value1) {
            this.Value1 = value1;
        }
    }

    public struct Param2 {
        public int Value1;
        public int Value2;

        public Param2(int value1, int value2) {
            this.Value1 = value1;
            this.Value2 = value2;
        }
    }

    public struct Param3 {
        public int Value1;
        public int Value2;
        public int Value3;

        public Param3(int value1, int value2, int value3) {
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
        }
    }

    public struct Param4 {
        public int Value1;
        public int Value2;
        public int Value3;
        public int Value4;

        public Param4(int value1, int value2, int value3, int value4) {
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
            this.Value4 = value4;
        }
    }

    // {string,string}键值对
    public struct ParamKV {
        public string Key;
        public string Value;

        public ParamKV(string key, string val) {
            this.Key = key;
            this.Value = val;
        }
    }

    public class StringHelper {
        //解析格式{1,10},{2,10}...

        public static List<string> RemoveEmpty(string[] arr) {
            List<string> retArr = new List<string>();
            for (int nIndex = 0; nIndex < arr.Length; ++nIndex) {
                string str = arr[nIndex];
                str = str.Trim();
                if (!string.IsNullOrEmpty(str)) {
                    retArr.Add(str);
                }
            }
            return retArr;
        }

        public static List<string> ParseStringList(string strInput) {

            List<string> retList = new List<string>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            if (strInpurArr.Length > 0) {
                string str = strInpurArr[0];
                str = str.Trim();
                if (str.Length < 2) {
                    return retList;
                }
                string[] arr = str.Split(t1);

                return RemoveEmpty(arr);
            }
            return retList;
        }

        /// <summary>
        /// 解析一个参数的列表{1}{1}{1}
        /// </summary>
        public static List<Param1> ParseParam1List(string strInput) {
            List<Param1> retList = new List<Param1>();
            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }
            char[] token = new char[] { ',', '，' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 1) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 1) {
                    int value1 = Convert.ToInt32(arrList[0]);
                    retList.Add(new Param1(value1));
                }
            }
            return retList;
        }

        /// <summary>
        /// 解析两个参数的列表{1,2}{1,2}{1,2}
        /// </summary>
        public static List<Param2> ParseParam2List(string strInput) {
            List<Param2> retList = new List<Param2>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 2) {
                    int value1 = Convert.ToInt32(arrList[0]);
                    int value2 = Convert.ToInt32(arrList[1]);
                    retList.Add(new Param2(value1, value2));
                }
            }
            return retList;
        }

        /// <summary>
        /// 解析 kv键值对{string,string}
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static List<ParamKV> ParseParamKVList(string strInput) {
            List<ParamKV> retList = new List<ParamKV>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 2) {
                    string value1 = arrList[0];
                    string value2 = arrList[1];
                    retList.Add(new ParamKV(value1, value2));
                }
            }
            return retList;
        }

        /// <summary>
        /// 解析三个参数的列表{1,2,3}{1,2,3}{1,2,3}
        /// </summary>
        public static List<Param3> ParseParam3List(string strInput) {
            List<Param3> retList = new List<Param3>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 3) {
                    int value1 = Convert.ToInt32(arrList[0]);
                    int value2 = Convert.ToInt32(arrList[1]);
                    int value3 = Convert.ToInt32(arrList[2]);
                    retList.Add(new Param3(value1, value2, value3));
                }
            }
            return retList;
        }
        public static List<Param4> ParseParam4List(string strInput) {
            List<Param4> retList = new List<Param4>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 4) {
                    int value1 = Convert.ToInt32(arrList[0]);
                    int value2 = Convert.ToInt32(arrList[1]);
                    int value3 = Convert.ToInt32(arrList[2]);
                    int value4 = Convert.ToInt32(arrList[3]);
                    retList.Add(new Param4(value1, value2, value3, value4));
                }
            }
            return retList;
        }

        /// <summary>
        /// 解析多列多个参数的列表{1,2}{1,2,3}{1,2,3,4}
        /// </summary>
        public static List<List<int>> ParseListIntList(string strInput) {

            List<List<int>> retList = new List<List<int>>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);
                List<int> nList = new List<int>();
                for (int i = 0; i < arrList.Count; i++) {
                    nList.Add(Convert.ToInt32(arrList[i]));
                }
                retList.Add(nList);
            }
            return retList;
        }

        /// <summary>
        /// 解析 多个参数列表{string}{string&string}{string&string&string} ...
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static List<List<string>> ParseListStringList(string strInput) {

            List<List<string>> retList = new List<List<string>>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ';', '{' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);
                List<string> nList = new List<string>();
                for (int i = 0; i < arrList.Count; i++) {
                    nList.Add(arrList[i]);
                }
                retList.Add(nList);
            }
            return retList;
        }

        /// <summary>
        /// 解析单列多个参数的列表{1,2,3,4,5,6...}
        /// </summary>
        public static List<int> ParseIntList(string strInput) {

            List<int> retList = new List<int>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            if (strInpurArr.Length > 0) {
                string str = strInpurArr[0];
                str = str.Trim();
                if (str.Length < 2) {
                    return retList;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                for (int i = 0; i < arrList.Count; i++) {
                    retList.Add(Convert.ToInt32(arrList[i]));
                }
            }
            return retList;
        }

        /// <summary>
        /// 解析单列多个参数的列表{1,2,3,4,5,6...}
        /// </summary>
        public static List<long> ParseInt64List(string strInput) {

            List<long> retList = new List<long>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            if (strInpurArr.Length > 0) {
                string str = strInpurArr[0];
                str = str.Trim();
                if (str.Length < 2) {
                    return retList;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                for (int i = 0; i < arrList.Count; i++) {
                    retList.Add(Convert.ToInt64(arrList[i]));
                }
            }
            return retList;
        }

        //解析1个参数的列表float
        public static List<float> ParseParamFloat1List(string strInput) {
            List<float> retList = new List<float>();
            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }
            char[] token = new char[] { ',', '，' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 1) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 1) {
                    float value1 = Convert.ToSingle(arrList[0]);
                    retList.Add(value1);
                }
            }
            return retList;
        }

        //解析两个参数的列表
        public static List<Vector2> ParseParamFloat2List(string strInput) {
            List<Vector2> retList = new List<Vector2>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 2) {
                    float value1 = Convert.ToSingle(arrList[0]);
                    float value2 = Convert.ToSingle(arrList[1]);
                    retList.Add(new Vector2(value1, value2));
                }
            }
            return retList;
        }

        //解析三个参数的列表
        public static List<Vector3> ParseParamFloat3List(string strInput) {
            List<Vector3> retList = new List<Vector3>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 3) {
                    float value1 = Convert.ToSingle(arrList[0]);
                    float value2 = Convert.ToSingle(arrList[1]);
                    float value3 = Convert.ToSingle(arrList[2]);
                    retList.Add(new Vector3(value1, value2, value3));
                }
            }
            return retList;
        }

        //解析四个参数的列表
        public static List<Vector4> ParseParamFloat4List(string strInput) {
            List<Vector4> retList = new List<Vector4>();

            if (string.IsNullOrEmpty(strInput)) {
                return retList;
            }

            char[] token = new char[] { '}' };
            strInput = strInput.Trim();
            string[] strInpurArr = strInput.Split(token);

            char[] t1 = new char[] { '}', ',', '，', '{', ' ', ':', '：' };
            for (int nIndex = 0; nIndex < strInpurArr.Length; ++nIndex) {
                string str = strInpurArr[nIndex];
                str = str.Trim();
                if (str.Length < 2) {
                    continue;
                }
                string[] arr = str.Split(t1);

                List<string> arrList = RemoveEmpty(arr);

                if (arrList.Count >= 3) {
                    float value1 = Convert.ToSingle(arrList[0]);
                    float value2 = Convert.ToSingle(arrList[1]);
                    float value3 = Convert.ToSingle(arrList[2]);
                    float value4 = Convert.ToSingle(arrList[3]);
                    retList.Add(new Vector4(value1, value2, value3, value4));
                }
            }
            return retList;
        }
    }
}
