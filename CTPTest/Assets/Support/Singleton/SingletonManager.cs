using UnityEngine;
using System.Collections.Generic;
namespace PSupport
{
    namespace PSingleton
    {
        /// <summary>
        /// 单例管理器,用来查找,删除,重置所有单例对象
        /// </summary>
        public class SingletonManager
        {
           
            /// <summary>
            /// 用来存储mono单例的gameobject
            /// </summary>
            internal static GameObject mgMonoContainer = null;
            /// <summary>
            /// 容器名字名字
            /// </summary>
            internal static string msContainerName = "SingletonMono";
            /// <summary>
            /// 用来存储单例对象的容器
            /// </summary>
            private  static Dictionary<string, object> mDicSingletonMap = new Dictionary<string, object>();

            public static bool isCreatedInstance(string sInstanceName)
            {
                if (mDicSingletonMap != null && mDicSingletonMap.ContainsKey(sInstanceName))
                {
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 增加单例实例
            /// </summary>
            /// <param name="sInstanceName"></param>
            /// <param name="instance"></param>
            internal static void _addInstance(string sInstanceName,object instance)
            {
                if (instance.GetType().IsAssignableFrom(typeof(MonoBehaviour)))
                {//如果是MonoBehaviour的实例
                    if (mgMonoContainer == null)
                    {
                        mgMonoContainer = new GameObject(msContainerName);
                        Object.DontDestroyOnLoad(mgMonoContainer);
                    }
                }
                if (!mDicSingletonMap.ContainsKey(sInstanceName))
                {
                    mDicSingletonMap.Add(sInstanceName, instance);
                }
            }
            /// <summary>
            /// 删除实例
            /// </summary>
            /// <param name="sInstanceName"></param>
            public static void removeInstance(string sInstanceName)
            {
                if (mDicSingletonMap.ContainsKey(sInstanceName))
                {
                    object instance = mDicSingletonMap[sInstanceName];
                    if (instance != null)
                    {
                        if (instance.GetType().IsAssignableFrom(typeof(MonoBehaviour)))
                        {//如果是MonoBehaviour的实例
                            Object.Destroy((Object)instance);
                        }
                        mDicSingletonMap.Remove(sInstanceName);
                    }
                    
                }
            }
            /// <summary>
            /// 清空单例管理器
            /// </summary>
            public static void clear()
            {
                if (mgMonoContainer != null)
                {
                    Object.Destroy(mgMonoContainer);
                    mgMonoContainer = null;
                }
                mDicSingletonMap.Clear();
            }

        }
    }

}


