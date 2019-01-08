using UnityEngine;

public static class GameConfigs
{
    //资源管理器 加载模式
    public static FoxGame.Asset.AssetLoadMode LoadAssetMode = FoxGame.Asset.AssetLoadMode.Editor;



#if UNITY_ANDROID
	static string curPlatformName = "android";
#elif UNITY_IPHONE
	static string curPlatformName = "iphone";
#else
    static string curPlatformName = "win";
#endif

    //当前平台名
    public static string CurPlatformName { get { return curPlatformName; } }


    //资源服务器url
    public static string ResServerUrl = "http://127.0.0.1/ResServer";


    //(该文件夹只能读,打包时被一起写入包内,第一次运行游戏把该文件夹数据拷贝到本地ab包路径下) 
    public static string StreamingAssetABRootPath = Application.streamingAssetsPath + "/" + curPlatformName;
    //streamingasset目录下的manifest文件路径
    public static string StreamingAssetManifestPath = Application.streamingAssetsPath + "/" + curPlatformName + "/" + curPlatformName;

    //游戏资源文件路径
    public static string GameResPath = Application.dataPath + "/GameRes";
    //打包资源的输出文件夹
    public static string GameResExportPath = Application.streamingAssetsPath + "/" + curPlatformName;

    

    //本地ab包根路径(该文件夹可读可写,从资源服务器更新的数据也放在这里)
    public static string LocalABRootPath = Application.persistentDataPath + "/DownLoad/" + curPlatformName;
    // 本地manifest文件路径
    public static string LocalManifestPath = Application.persistentDataPath + "/DownLoad/" + curPlatformName + "/" + curPlatformName;

    
    //资源服务器ab包根路径
    public static string OnlineABRootPath = ResServerUrl + "/" + curPlatformName;
    //资源服务器manifest文件路径
    public static string OnlineManifestPath = ResServerUrl + "/" + curPlatformName + "/"+curPlatformName;
    
    



    //临时数据存放 缓存
    //public static string TmpPath = Application.temporaryCachePath + "/Cache/" + curPlatformName;

    //服务器版本号url
    public static string ServerVersionUrl = "http://127.0.0.1/ResServer/version.txt";









    #region game res path
    private static string AssetRoot {
        get {
            if (LoadAssetMode == FoxGame.Asset.AssetLoadMode.Editor) { //编辑器加载资源模式
                return GameResPath;
            } else {
                return LocalABRootPath;
            }
        }
    }

    //ui预制体路径
    public static string GetUIPath(string prefabName) {
        string str = "/Prefabs/UI/" + prefabName;
        if (LoadAssetMode != FoxGame.Asset.AssetLoadMode.Editor) {
            str = str.ToLower();
        } else {
            str = str + ".prefab";
        }
        return AssetRoot + str;
    }

    //音效路径
    public static string GetAudioPath(string name) {
        string str = "/Audios/" + name;
        if (LoadAssetMode != FoxGame.Asset.AssetLoadMode.Editor) {
            str = str.ToLower();
        } else {
            str = str + ".mp3";
        }
        return AssetRoot + str;
    }

    //图集路径
    public static string GetSpriteAtlasPath(string name) {
        string str = "/Atlas/" + name;
        if (LoadAssetMode != FoxGame.Asset.AssetLoadMode.Editor) {
            str = str.ToLower();
        } else {
            str = str + ".spriteatlas";
        }
        return AssetRoot + str;
    }

    //游戏配置路径
    public static string GetConfigPath(string name) {
        string str = "/Config/" + name;
        if (LoadAssetMode != FoxGame.Asset.AssetLoadMode.Editor) {
            str = str.ToLower();
        } else {
            str = str + ".bytes";
        }
        return AssetRoot + str;
    }

    #endregion
}
