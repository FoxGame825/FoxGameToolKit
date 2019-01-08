/* 
 * Unless otherwise licensed, this file cannot be copied or redistributed in any format without the explicit consent of the author.
 * (c) Preet Kamal Singh Minhas, http://marchingbytes.com
 * contact@marchingbytes.com
 */
using UnityEngine;
using System.Collections;

namespace FoxGame.Utils {
	public class PoolObject : MonoBehaviour {
        [HideInInspector]
		public string poolName;
		//defines whether the object is waiting in pool or is in use
        [HideInInspector]
		public bool isPooled;

        public Pool MyPool;

        //不好获取EasyObjectPool引用时 可以调用该函数
        public void Despawn()
        {
            if (MyPool != null)
            {
                MyPool.myParent.Despawn(this.gameObject);
            }
        }
	}

}
