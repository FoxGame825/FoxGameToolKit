using UnityEngine;
using Object = UnityEngine.Object;

namespace FoxGame.Asset
{
    //缓冲数据信息
    public class CacheDataInfo
    {
        public float StartTick;  //进入缓冲区时间
        public Object CacheObj; //缓冲物体
        public string CacheName;    //缓冲数据名称

        public CacheDataInfo(string name,Object obj) {
            CacheName = name;
            CacheObj = obj;
        }

        //刷新进入缓冲区时间
        public void UpdateTick() {
            StartTick = Time.realtimeSinceStartup;
        }
    }

}
