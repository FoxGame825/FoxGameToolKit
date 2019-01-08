using System.Collections.Generic;
using UnityEngine;
using FoxGame;
using UnityEngine.Events;
using System.Linq;
using Object = UnityEngine.Object;

namespace FoxGame.Asset
{

    //资源管理器 (资源管理器 仅会把资源加载入内存)
    public class AssetManager : MonoSingleton<AssetManager>
    {
        [Header("当前资源加载模式:")]
        public AssetLoadMode LoadMode = AssetLoadMode.Editor;
        [Header("定时清理缓冲间隔(秒):")]
        public float ClearCacheDuration;
        [Header("缓存数据驻留时间(秒)")]
        public float CacheDataStayTime;

        private IAssetLoader editorLoader;
        private IAssetLoader abLoader;

        private float cacheTimeTemp;

        //缓冲区[key 为绝对路径]
        private Dictionary<string, CacheDataInfo> cacheDataDic = new Dictionary<string, CacheDataInfo>();


        public void InitMode(AssetLoadMode mode, float duration = 120f, float cacheStayTime = 100f) {
            Debug.LogFormat("[AssetManager]初始化 当前加载模式:{0} 定时清理缓冲间隔:{1}s 缓存驻留时间:{2}s", mode, duration, cacheStayTime);
            LoadMode = mode;
            ClearCacheDuration = duration;
            CacheDataStayTime = cacheStayTime;
            editorLoader = new EditorAssetLoader();
            abLoader = new AssetBundleLoader(GameConfigs.LocalABRootPath, GameConfigs.LocalManifestPath);
        }

        //同步加载
        public T LoadAsset<T>(string path) where T : Object {
            CacheDataInfo info = queryCache(path);
            if (info != null) {
                info.UpdateTick();
                return info.CacheObj as T;
            } else {
                switch (LoadMode) {
                    case AssetLoadMode.Editor:
                        return editorLoader.LoadAsset<T>(path);
                    case AssetLoadMode.AssetBundler:
                        return abLoader.LoadAsset<T>(path);
                }
                return null;
            }
        }

        //异步加载
        public void LoadAssetAsync<T>(string path, UnityAction<T> onLoadComplate) where T : Object {

            CacheDataInfo info = queryCache(path);
            if (info != null) {
                info.UpdateTick();
                if (onLoadComplate != null) {
                    onLoadComplate(info.CacheObj as T);
                }
            } else {
                switch (LoadMode) {
                    case AssetLoadMode.Editor:
                        StartCoroutine(editorLoader.LoadAssetAsync<T>(path, onLoadComplate));
                        break;
                    case AssetLoadMode.AssetBundler:
                        StartCoroutine(abLoader.LoadAssetAsync<T>(path, onLoadComplate));
                        break;
                }

            }
        }

        //检测缓冲区
        private CacheDataInfo queryCache(string path) {
            if (cacheDataDic.ContainsKey(path)) {
                return cacheDataDic[path];
            }
            return null;
        }

        //加入缓冲区
        public void pushCache(string path,Object obj) {
            Debug.Log("[AssetManager]加入缓存:" + path);

            lock (cacheDataDic) {
                if (cacheDataDic.ContainsKey(path)) {
                    cacheDataDic[path].UpdateTick();
                } else {
                    CacheDataInfo info = new CacheDataInfo(path, obj);
                    cacheDataDic.Add(path, info);
                    info.UpdateTick();
                }
            }
        }

        //清空缓冲区
        public void RemoveCache() {
            cacheDataDic.Clear();
        }

        //清理缓冲区
        private void updateCache() {
            Debug.Log("[AssetManager]清理缓存");
            foreach (var iter in cacheDataDic.ToList()) {
                if (iter.Value.StartTick + CacheDataStayTime >= Time.realtimeSinceStartup) {
                    Debug.Log("过期清理:" + iter.Value.CacheName);
                    cacheDataDic.Remove(iter.Key);
                }
            }
        }


        private void Update() {
            if (ClearCacheDuration < 0) return;
            cacheTimeTemp += Time.deltaTime;

            if(cacheTimeTemp >= ClearCacheDuration) {
                updateCache();
                cacheTimeTemp -= ClearCacheDuration;
            }
        }

    }

}
