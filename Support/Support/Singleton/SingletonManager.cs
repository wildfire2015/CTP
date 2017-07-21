using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

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
            internal static string msContainerName = "SingletonManager";
            /// <summary>
            /// 用来存储单例对象的容器
            /// </summary>
            private  static List<string> mListSingleton = new List<string>();

            public static bool isCreatedInstance(string sInstanceName)
            {
                if (mListSingleton != null && mListSingleton.Contains(sInstanceName))
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
            internal static void _addInstance(string sInstanceName)
            {
                if (System.Type.GetType(sInstanceName).IsAssignableFrom(typeof(MonoBehaviour)))
                {//如果是MonoBehaviour的实例
                    if (mgMonoContainer == null)
                    {
                        mgMonoContainer = new GameObject(msContainerName);
                        Object.DontDestroyOnLoad(mgMonoContainer);
                    }
                }
                if (!mListSingleton.Contains(sInstanceName))
                {
                    mListSingleton.Add(sInstanceName);
                }
            }
            /// <summary>
            /// 增加非mono单例实例
            /// </summary>
            /// <param name="sInstanceName"></param>
            /// <param name="instance"></param>
            internal static T _addMonoInstance<T>() where T:MonoBehaviour
            {
                T instance = null;
                if (mgMonoContainer == null)
                {
                    mgMonoContainer = new GameObject(msContainerName);
                    Object.DontDestroyOnLoad(mgMonoContainer);
                }
                instance = mgMonoContainer.AddComponent<T>();
                if (instance != null)
                {
                    string typename = typeof(T).FullName;
                    if (!mListSingleton.Contains(typename))
                    {
                        mListSingleton.Add(typename);
                    }
                }
                return instance;
            }
            /// <summary>
            /// 删除实例
            /// </summary>
            /// <param name="sInstanceName"></param>
            public static void removeInstance(string sInstanceName)
            {
                if (mListSingleton.Contains(sInstanceName))
                {
                    System.Type type = System.Type.GetType(sInstanceName);
                    System.Type typebase = null;
                    bool bIsMono = type.IsSubclassOf(typeof(MonoBehaviour));
                    if (bIsMono)
                    {//如果是MonoBehaviour的实例
                        typebase = typeof(SingletonMono<>);
                        Component c = mgMonoContainer.GetComponent(sInstanceName);
                        Object.Destroy(c);
                    }
                    else
                    {
                        typebase = typeof(Singleton<>);
                    }
                    typebase = typebase.MakeGenericType(type);
                    //清理对应类的静态实例对象
                    FieldInfo proinfo = typebase.GetField("msInstance", BindingFlags.NonPublic | BindingFlags.Static);
                    if (proinfo != null)
                    {
                        ConstructorInfo Constructor = typebase.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new System.Type[0], null);
                        object instance = Constructor.Invoke(null);
                        proinfo.SetValue(instance, null);
                    }
                    
                    mListSingleton.Remove(sInstanceName);
                    if (mListSingleton.Count == 0)
                    {
                        if (mgMonoContainer != null)
                        {
                            Object.Destroy(mgMonoContainer);
                            mgMonoContainer = null;
                        }
                    }
                }
            }
            /// <summary>
            /// 清空单例管理器
            /// </summary>
            public static void clear()
            {
                string[] singletons = mListSingleton.ToArray();
                for (int i = 0; i < singletons.Length; i++)
                {
                    removeInstance(singletons[i]);
                }
            }

        }
    }

}


