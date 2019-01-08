using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;


namespace FoxGame.Asset
{
    //编辑器模式加载器[编辑器模式 同步异步无区别]
    public class EditorAssetLoader : IAssetLoader
    {

        public T LoadAsset<T>(string path) where T : class {
            return load<T>(path);
        }

        public IEnumerator LoadAssetAsync<T>(string path, UnityAction<T> callback) where T : class {
            if (callback != null) {
                callback(load<T>(path));
            }
            yield return null;
        }


        T load<T>(string path) where T : class {
#if UNITY_EDITOR
            string absolutepath = path;

            //绝对路径转为相对Assets文件夹的相对路径
            path = PathUtils.GetRelativePath(path, Application.dataPath);
            Debug.Log("[LoadAsset(Editor)]: " + path);
            Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if (obj == null) {
                Debug.LogError("Asset not found - path:" + path);
            }

            AssetManager.Instance.pushCache(absolutepath, obj);
            return obj as T;
#endif
            return null;
        }
    }

}
