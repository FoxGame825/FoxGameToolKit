using UnityEngine.Events;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace FoxGame.Asset
{
    //ab资源加载器
    public class AssetBundleLoader : IAssetLoader
    {
        private string assetRootPath; //ab包根路径
        private string mainfastPath;
        private static AssetBundleManifest manifest;

        public AssetBundleLoader(string assetPath,string mainfast) {
            assetRootPath = assetPath;
            mainfastPath = mainfast;
        }


        public T LoadAsset<T>(string path) where T : class {
            string absolutepath = path;

            path = PathUtils.NormalizePath(path);
           
            Debug.Log("[LoadAsset]: " + path);
            //打的ab包都资源名称和文件名都是小写的
            string assetBundleName = PathUtils.GetAssetBundleNameWithPath(path, assetRootPath);

            //1加载Manifest文件
            LoadManifest();

            //2获取文件依赖列表
            string[] dependencies = manifest.GetAllDependencies(assetBundleName);

            //3加载依赖资源
            List<AssetBundle> assetbundleList = new List<AssetBundle>();
            foreach (string fileName in dependencies) {
                string dependencyPath = assetRootPath + "/" + fileName;
                Debug.Log("[加载1 依赖资源]: " + dependencyPath);
                assetbundleList.Add(AssetBundle.LoadFromFile(dependencyPath));
            }
            //4加载目标资源
            //Object obj = null;
            AssetBundle assetBundle = null;
            Debug.Log("[加载2 目标资源]: " + path);
            assetBundle = AssetBundle.LoadFromFile(path);
            assetbundleList.Insert(0, assetBundle);

            Object obj = assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(path), typeof(T));
            //5释放目标资源
            //Debug.Log("---释放目标资源:" + path);

            //6释放依赖资源
            UnloadAssetbundle(assetbundleList);

            AssetManager.Instance.pushCache(absolutepath, obj);

            return obj as T;
        }

        public IEnumerator LoadAssetAsync<T>(string path, UnityAction<T> callback) where T : class {
            string absolutepath = path;
            path = PathUtils.NormalizePath(path);


            Debug.Log("[LoadAssetAsync]: " + path);
            //打的ab包都资源名称和文件名都是小写的
            string assetBundleName = PathUtils.GetAssetBundleNameWithPath(path, assetRootPath);
            //1加载Manifest文件
            LoadManifest();
            //2获取文件依赖列表
            string[] dependencies = manifest.GetAllDependencies(assetBundleName);
            //3加载依赖资源
            AssetBundleCreateRequest createRequest;
            List<AssetBundle> assetbundleList = new List<AssetBundle>();
            foreach (string fileName in dependencies) {
                string dependencyPath = assetRootPath + "/" + fileName;

                Debug.Log("[加载1 依赖资源]: " + dependencyPath);
                createRequest = AssetBundle.LoadFromFileAsync(dependencyPath);
                yield return createRequest;
                if (createRequest.isDone) {
                    assetbundleList.Add(createRequest.assetBundle);

                } else {
                    Debug.Log("加载依赖资源出错");
                }

            }
            //4加载目标资源
            AssetBundle assetBundle = null;
            Debug.Log("[加载2 目标资源]: " + path);
            createRequest = AssetBundle.LoadFromFileAsync(path);
            yield return createRequest;
            if (createRequest.isDone) {
                assetBundle = createRequest.assetBundle;
                //5释放目标资源
                assetbundleList.Insert(0, assetBundle);
            }
            AssetBundleRequest abr = assetBundle.LoadAssetAsync(Path.GetFileNameWithoutExtension(path), typeof(T));
            yield return abr;
            Object obj = abr.asset;

            AssetManager.Instance.pushCache(absolutepath, obj);

            callback(obj as T);

            //yield return null;
            //yield return null;
            //6释放依赖资源
            UnloadAssetbundle(assetbundleList);
            //Debug.Log("---end loadAsync:AssetBundleLoader.loadAsync" + path);

        }


        /// <summary>
        /// 加载资源配置文件
        /// </summary>
        private void LoadManifest() {
            if (manifest == null) {
                string path = mainfastPath;
                Debug.Log("start load Manifest:" + path);

                AssetBundle manifestAB = AssetBundle.LoadFromFile(path);
                manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                manifestAB.Unload(false);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="list"></param>
        private void UnloadAssetbundle(List<AssetBundle> list) {
            //为了解决在ios上同步加载后直接释放造成加载出来的资源被回收的问题，需要隔一帧再释放
            for (int i = 0; i < list.Count; i++) {
                list[i].Unload(false);
            }
            list.Clear();
        }
    }

}
