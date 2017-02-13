using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
namespace PSupport
{
    namespace PSingleton
    {
        /// <summary>
        /// 非线程安全MonoBehaviour单例
        /// </summary>
        public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
        {
            //实例
            private static T msInstance = null;
            private static string msTName = "";
            public static T instance
            {
                get
                {
                    if (msInstance == null)
                    {
                        msTName = typeof(T).FullName;
                        msInstance = SingletonManager.mgMonoContainer.AddComponent<T>();
                        if (msInstance == null)
                        {
                            throw new System.InvalidOperationException(string.Format("The SingleTon MonoBehaviour for {0} create failed!", msTName));
                        }
                        SingletonManager._addInstance(msTName, msInstance);
                    }
                    return msInstance;
                }
                
            }
            private void Awake()
            {
                if (gameObject.name != SingletonManager.msContainerName)
                {//如果单例mono类加入到其他物体上,则强制删除
                    ReleaseInstance();
                    throw new System.InvalidOperationException(string.Format("The SingleTon MonoBehaviour for {0} can only be unique", typeof(T)));
                    
                }
            }
            public static void ReleaseInstance()
            {
                SingletonManager.removeInstance(msTName);
            }

        }
    }
    
}


