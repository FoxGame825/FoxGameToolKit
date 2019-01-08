using UnityEngine;

namespace FoxGame
{

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {

        private static T m_Instance = null;

        private bool m_bInit = false;

        public static T Instance {
            get {
                if (m_Instance == null) {
                    GameObject obj = GameObject.Find(typeof(T).Name);
                    if (obj == null) {
                        obj = new GameObject(typeof(T).Name);
                        m_Instance = obj.AddComponent<T>();
                    } else {
                        m_Instance = obj.GetComponent<T>();
                        if (m_Instance == null) {
                            m_Instance = obj.AddComponent<T>();
                        }
                    }

                    if (m_Instance.m_bInit == false) {
                        m_Instance.Init();
                    }
                }
                return m_Instance;
            }
        }

        private void Awake() {
            if (m_Instance == null) {
                m_Instance = this as T;
                //DontDestroyOnLoad(m_Instance.gameObject);
                if (m_Instance.m_bInit == false) {
                    m_Instance.Init();
                }
            }
        }

        public virtual void Init() {
            DontDestroyOnLoad(m_Instance.gameObject);
            m_Instance.m_bInit = true;
        }

        private void OnApplicationQuit() {
            m_Instance = null;
        }
    }
}