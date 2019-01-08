/* 
 * Unless otherwise licensed, this file cannot be copied or redistributed in any format without the explicit consent of the author.
 * (c) Preet Kamal Singh Minhas, http://marchingbytes.com
 * contact@marchingbytes.com
 */
using UnityEngine;

namespace FoxGame.Utils
{
    [System.Serializable]
	public class PoolInfo {
		public string poolName;
		public GameObject prefab;
		public int poolSize;
		public bool fixedSize;
	}
}
