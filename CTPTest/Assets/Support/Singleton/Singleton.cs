using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace PSupport
{
    namespace PSingleton
    {
        /// <summary>
        /// 多线程安全泛型单例类,派生类必须实现私有默认构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Singleton<T> where T : class
        {
            //private Singleton() { }
            /// <summary>
            /// 单例实例
            /// </summary>
            private static T msInstance = null;
            /// <summary>
            /// 类名字
            /// </summary>
            private static string msTName = "";
            /// <summary>
            /// 空参数
            /// </summary>
            private static System.Type[] msEmptyTypes = new System.Type[0];

            /// <summary>
            /// 线程安全锁
            /// </summary>
            private static readonly object mLockSingleton = new object();

            /// <summary>
            /// 获取单例实例
            /// </summary>
            public static T instance
            {
                get
                {
                    if (msInstance == null)
                    {
                        lock (mLockSingleton)
                        {//double-check
                            if (msInstance == null)
                            {
                                msTName = typeof(T).FullName;
                                ConstructorInfo Constructor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, msEmptyTypes, null);
                                if (Constructor == null)
                                {
                                    throw new System.InvalidOperationException(string.Format("The constructor for {0} must be private and take no parameters.", typeof(T)));
                                }
                                msInstance = (T)Constructor.Invoke(null);
                            }
                            
                        }

                    }
                    SingletonManager._addInstance(msTName);
                    return msInstance;

                }
            }
            /// <summary>
            /// 释放实例
            /// </summary>
            public static void ReleaseInstance()
            {
                SingletonManager.removeInstance(msTName);
            }

        }
    }
    

}
