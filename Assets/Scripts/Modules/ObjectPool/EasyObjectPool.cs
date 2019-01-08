/* 
 * Unless otherwise licensed, this file cannot be copied or redistributed in any format without the explicit consent of the author.
 * (c) Preet Kamal Singh Minhas, http://marchingbytes.com
 * contact@marchingbytes.com
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FoxGame.Utils
{

    /// <summary>
    /// Easy object pool.
    /// </summary>
    public class EasyObjectPool : MonoBehaviour {

		//public static EasyObjectPool instance;
		[Header("Editing Pool Info value at runtime has no effect")]
		public PoolInfo[] poolInfo;

		//mapping of pool name vs list
		protected Dictionary<string, Pool> poolDictionary  = new Dictionary<string, Pool>();
		

        private void Awake() {
            //check for duplicate names
            CheckForDuplicatePoolNames();
            //create pools
            CreatePools();
        }

        private void CheckForDuplicatePoolNames() {
			for (int index = 0; index < poolInfo.Length; index++) {
				string poolName = poolInfo[index].poolName;
				if(poolName.Length == 0) {
					Debug.LogError(string.Format("Pool {0} does not have a name!",index));
				}
				for (int internalIndex = index + 1; internalIndex < poolInfo.Length; internalIndex++) {
					if(poolName.Equals(poolInfo[internalIndex].poolName)) {
						Debug.LogError(string.Format("Pool {0} & {1} have the same name. Assign different names.", index, internalIndex));
					}
				}
			}
		}

        //子类可重写
		protected virtual void CreatePools() {
			foreach (PoolInfo currentPoolInfo in poolInfo) {
				
				Pool pool = new Pool(currentPoolInfo.poolName, currentPoolInfo.prefab, 
				                     currentPoolInfo.poolSize, currentPoolInfo.fixedSize,this);

				
				Debug.Log("Creating pool: " + currentPoolInfo.poolName);
				//add to mapping dict
				poolDictionary[currentPoolInfo.poolName] = pool;
			}
		}


		/* Returns an available object from the pool 
		OR 
		null in case the pool does not have any object available & can grow size is false.
		*/
		private GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation) {
			GameObject result = null;
			
			if(poolDictionary.ContainsKey(poolName)) {
				Pool pool = poolDictionary[poolName];
                //result = pool.NextAvailableObject(position,rotation);
                result = pool.NextAvailableObject();
                //scenario when no available object is found in pool
                if (result == null) {
					Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
				}
				
			} else {
				Debug.LogError("Invalid pool name specified: " + poolName);
			}
			
			return result;
		}

        private GameObject GetObjectFromPool(string poolName)
        {
            GameObject result = null;

            if (poolDictionary.ContainsKey(poolName))
            {
                Pool pool = poolDictionary[poolName];
                //result = pool.NextAvailableObject(position,rotation);
                result = pool.NextAvailableObject();
                //scenario when no available object is found in pool
                if (result == null)
                {
                    Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
                }

            }
            else
            {
                Debug.LogError("Invalid pool name specified: " + poolName);
            }

            return result;
        }

        public T Spawn<T>(string poolName,Vector3 position,Quaternion rotation)
        {
            GameObject result = GetObjectFromPool(poolName,position,rotation);
            if (result != null) {
                T t = result.GetComponent<T>();
                if (t == null) {
                    Debug.LogWarning("No Component available in object. Consider setting fixedSize to false.: " + poolName);
                    return default(T);
                }
                return t;
            }
            return default(T);
        }

        public GameObject Spawn(string poolName) {
            GameObject result = GetObjectFromPool(poolName);
            if (result != null) {
                return result;
            }
            return null;
        }

        public T Spawn<T>(string poolName)
        {
            GameObject result = GetObjectFromPool(poolName);
            if (result != null) {
                T t = result.GetComponent<T>();
                if (t == null) {
                    Debug.LogWarning("No Component available in object. Consider setting fixedSize to false.: " + poolName);
                    return default(T);
                }
                return t;
            }
            return default(T);
        }

        private void ReturnObjectToPool(GameObject go) {
			PoolObject po = go.GetComponent<PoolObject>();
			if(po == null) {
				Debug.LogWarning("Specified object is not a pooled instance: " + go.name);
			} else {
				if(poolDictionary.ContainsKey(po.poolName)) {
					Pool pool = poolDictionary[po.poolName];
					pool.ReturnObjectToPool(po);
				} else {
					Debug.LogWarning("No pool available with name: " + po.poolName);
				}
			}
		}

        public void Despawn(GameObject go)
        {
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                Debug.LogWarning("Specified object is not a pooled instance: " + go.name);
            }
            else
            {
                if (poolDictionary.ContainsKey(po.poolName))
                {
                    Pool pool = poolDictionary[po.poolName];
                    pool.ReturnObjectToPool(po);
                }
                else
                {
                    Debug.LogWarning("No pool available with name: " + po.poolName);
                }
            }
        }
	}
}
