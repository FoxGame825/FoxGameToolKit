using System;

namespace FoxGame.Utils {

    public class Singleton<T> where T : class, new() {
        private static T _Instance;
        public static T Instance {
            get {
                if (Singleton<T>._Instance == null) {
                    Singleton<T>._Instance = Activator.CreateInstance<T>();
                }
                return Singleton<T>._Instance;
            }
        }
        static Singleton() {
            Singleton<T>._Instance = Activator.CreateInstance<T>();
        }
        public static void CreateInstance() {
            if (Singleton<T>._Instance == null) {
                Singleton<T>._Instance = Activator.CreateInstance<T>();
            }
        }
        public static void DestroyInstance() {
            if (Singleton<T>._Instance != null) {
                Singleton<T>._Instance = (T)((object)null);
            }
        }
        public static T GetInstance() {
            if (Singleton<T>._Instance == null) {
                Singleton<T>._Instance = Activator.CreateInstance<T>();
            }
            return Singleton<T>._Instance;
        }
    }

}