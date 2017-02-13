using UnityEngine;
using System.Collections;
using PSupport.PSingleton;
namespace PSupport
{
    namespace MTLoadSystem
    {
        /// <summary>
        /// 类名: MTResourceLoadManager
        /// 功能: 多线程资源加载管理器
        /// 作者: 彭谦
        /// 日期: 2017.2.9
        /// 修改:
        /// 备注:多线程资源加载管理器,管理资源加载过程中要处理的bundle加载,释放,回调,资源的删除等操作
        /// </summary>
        public class MTResourceLoadManager : Singleton<MTResourceLoadManager>
        {
            private MTResourceLoadManager() { }
            public int a = 1;
        }

    }


}
