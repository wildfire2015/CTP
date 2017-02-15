using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PSupport.PSingleton;
namespace PSupport
{
    namespace MTLoadSystem
    {
        #region MTResourceLoadManager类
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
            #region 内部类
            /// <summary>
            /// 加载管理器的设置
            /// </summary>
            public class LoadSetting
            {
                /// <summary>
                /// 是否读取打包资源
                /// </summary>
                public bool mbUseBundle = false;
                /// <summary>
                /// StreamingAssets 路径
                /// </summary>
                public string msStreamingAssetsPath = "";
            }
            #endregion

            #region 属性
            /// <summary>
            /// 回调函数的委托
            /// </summary>
            /// <param name="obj">回调参数</param>
            /// <param name="loadedNotify">加载结果</param>
            public delegate void ProcessDelegateArgc(object obj = null, eLoadedNotify loadedNotify = eLoadedNotify.Load_AllSuccessfull);

            /// <summary>
            /// 加载模块的设置
            /// </summary>
            private LoadSetting _mLoadSetting = new LoadSetting();

            /// <summary>
            /// 本地bundle配置信息
            /// </summary>
            internal static BundleInfoConfig _mLocalBundleInfoConfig = null;
            #endregion

            #region 接口部分

            #region 公共接口

            /// <summary>
            /// 设置LoadSetting
            /// </summary>
            public LoadSetting Setting
            {
                get
                {
                    return _mLoadSetting;
                }
                set
                {
                    _mLoadSetting = value;
                }
            }

            /// <summary>
            /// 请求资源,路径都为从Assets/Resources开始
            /// </summary>
            /// <param name="sinputpath"></param>
            /// <param name="type"></param>
            /// <param name=""></param>
            /// <param name="proc"></param>
            /// <param name="o"></param>
            public void requestRes(string sinputpath, System.Type type, ProcessDelegateArgc proc = null, object o = null)
            {

            }

            #endregion

            #region 非公共接口

            private MTResourceLoadManager() { }
            
            
            /// <summary>
            /// 加载bundle
            /// </summary>
            /// <param name="sloadpath"></param>
            private void _loadBundleStream(string sloadpath)
            {
                
            }
            #endregion

            #endregion
        }
        #endregion

        #region 命名空间下的枚举和类


        /// <summary>
        /// 加载结果
        /// </summary>
        public enum eLoadedNotify
        {
            /// <summary>
            /// 加载一个失败
            /// </summary>
            Load_OneFailed,
            /// <summary>
            /// 加载一个成功
            /// </summary>
            Load_OneSuccessfull,
            /// <summary>
            /// 加载全部完成
            /// </summary>
            Load_AllSuccessfull,
            /// <summary>
            /// 加载全部完成
            /// </summary>
            Load_NotAllSuccessfull
        }
        /// <summary>
        /// 所有的bundle配置信息
        /// </summary>
        public class BundleInfoConfig
        {
            /// <summary>
            /// 每个bundle的信息
            /// </summary>
            private class BundleInfo
            {
                public uint muSize;
                public string msMD5;
                public List<string> mListDepdenceBundleName = null;
            }
            private Dictionary<string, BundleInfo> _mDicBundleInfoConfig = new Dictionary<string, BundleInfo>();
            /// <summary>
            /// 初始化bundleconfig
            /// </summary>
            /// <param name="text"></param>
            public void initBundleInfoConfig(string text)
            {
                StringReader sr = new StringReader(text);
                int num = int.Parse(sr.ReadLine());
                sr.ReadLine();
                for (int i = 0; i < num; i++)
                {
                    BundleInfo binfo = new BundleInfo();
                    string bundlepath = sr.ReadLine();
                    uint size = uint.Parse(sr.ReadLine());
                    string md5 = sr.ReadLine();
                    binfo.muSize = size;
                    binfo.msMD5 = md5;
                    int depnum = int.Parse(sr.ReadLine());
                    if (depnum != 0)
                    {
                        binfo.mListDepdenceBundleName = new List<string>();
                    }
                    for (int d = 0; d < depnum; d++)
                    {
                        string depbundlepath = sr.ReadLine();
                        if (!binfo.mListDepdenceBundleName.Contains(depbundlepath))
                        {
                            binfo.mListDepdenceBundleName.Add(depbundlepath);
                        }
                    }
                    if (!_mDicBundleInfoConfig.ContainsKey(bundlepath))
                    {
                        _mDicBundleInfoConfig.Add(bundlepath, binfo);
                    }
                    sr.ReadLine();
                }
            }
            /// <summary>
            /// 获取所有的bundle
            /// </summary>
            /// <returns></returns>
            public string[] GetAllAssetBundles()
            {
                if (_mDicBundleInfoConfig.Count != 0)
                {
                    return new List<string>(_mDicBundleInfoConfig.Keys).ToArray();
                }
                return new string[0];

            }
            /// <summary>
            /// 获取bundle的依赖bundle
            /// </summary>
            /// <param name="bundlepath"></param>
            /// <returns></returns>
            public string[] GetAllDependencies(string bundlepath)
            {
                if (_mDicBundleInfoConfig.ContainsKey(bundlepath))
                {
                    if (_mDicBundleInfoConfig[bundlepath].mListDepdenceBundleName != null)
                    {
                        return _mDicBundleInfoConfig[bundlepath].mListDepdenceBundleName.ToArray();
                    }

                }
                return new string[0];

            }
            /// <summary>
            /// 判断是否包含此bundle
            /// </summary>
            /// <param name="bundlepath"></param>
            /// <returns></returns>
            public bool IsContainsBundle(string bundlepath)
            {
                return _mDicBundleInfoConfig.ContainsKey(bundlepath);
            }
            /// <summary>
            /// 获取bundle的md5
            /// </summary>
            /// <param name="bundlepath"></param>
            /// <returns></returns>
            public string getBundleMD5(string bundlepath)
            {
                if (_mDicBundleInfoConfig.ContainsKey(bundlepath))
                {
                    return _mDicBundleInfoConfig[bundlepath].msMD5;
                }
                return "";
            }
            /// <summary>
            /// 获取bundle的大小
            /// </summary>
            /// <param name="bundlepath"></param>
            /// <returns></returns>
            public uint getBundleSize(string bundlepath)
            {
                if (_mDicBundleInfoConfig.ContainsKey(bundlepath))
                {
                    return _mDicBundleInfoConfig[bundlepath].muSize;
                }
                return 0;
            }
        }
        #endregion
    }


}
