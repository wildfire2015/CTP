using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PSupport.PSingleton;
using UnityEngine;

namespace PSupport
{
    namespace MTLoadSystem
    {


        /// <summary>
        /// 回调函数的委托
        /// </summary>
        /// <param name="obj">回调参数</param>
        /// <param name="loadedNotify">加载结果</param>
        public delegate void ProcessDelegateArgc(object obj = null, eLoadedNotify loadedNotify = eLoadedNotify.Load_AllSuccessfull);

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
                /// <summary>
                /// 资源Cache目录标志,在固定的cache目录中,可自定义选择那个cache文件夹下为cache目录
                /// </summary>
                public string msCachePath = "Default";
            }
            #endregion

            #region 属性
           

            /// <summary>
            /// 加载模块的设置
            /// </summary>
            private LoadSetting _mLoadSetting = new LoadSetting();

            /// <summary>
            /// 本地bundle配置信息
            /// </summary>
            internal static BundleInfoConfig _mLocalBundleInfoConfig = null;

            /// <summary>
            /// 已经加载完的资源
            /// </summary>
            private static Dictionary<string, Hashtable> _mDicLoadedRes = new Dictionary<string, Hashtable>();


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


            public void UnCompressStreamingPathToCache(ProcessDelegateArgc proc = null, object o = null)
            {

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
                if (_mLocalBundleInfoConfig == null)
                {

                }
            }
            

            #endregion

            #region 非公共接口

            private MTResourceLoadManager() { }

            private void _requestRes(string sinputpath, System.Type type, ProcessDelegateArgc proc = null, object o = null)
            {

            }
            /// <summary>
            /// 加载bundle
            /// </summary>
            /// <param name="sloadpath"></param>
            private void _thread_LoadBundleStream(string sloadpath)
            {
                byte[] bytes = File.ReadAllBytes(sloadpath);
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
        /// 枚举3种读取资源的来源
        /// </summary>
        public enum eLoadResPath
        {
            /// <summary>
            /// 读取Resources下的资源
            /// </summary>
            RP_FromResources,
            /// <summary>
            /// 读取本地StreamingAssets下的资源
            /// </summary>
            RP_FromStreamingAssets,
            /// <summary>
            /// 读取网络路径,没有就从读取本地StreamingAssets下的资源里读取
            /// </summary>
            RP_FromURLFirst,
            /// <summary>
            /// 未知
            /// </summary>
            RP_Unknow

        }

        /// <summary>
        /// 加载参数
        /// </summary>
        internal class CloadParam
        {
            /// <summary>
            /// 加载资源路径
            /// </summary>
            public string[] mpaths = null;
            /// <summary>
            /// 加载资源类型
            /// </summary>
            public System.Type[] mtypes = null;
            /// <summary>
            /// 加载资源位置
            /// </summary>
            public eLoadResPath[] meloadResTypes = null;
            /// <summary>
            /// 加载资源tag
            /// </summary>
            public string[] mtags = null;
            /// <summary>
            /// 加载完毕回调
            /// </summary>
            public ProcessDelegateArgc mproc = null;
            public object mo = null;
            public bool mbasyn = true;
            public bool mbautoreleasebundle = true;
            public bool mbloadfromfile = true;

            public CloadParam(string spath, System.Type type, eLoadResPath eloadResType = eLoadResPath.RP_FromURLFirst, string tag = "", ProcessDelegateArgc proc = null, object o = null, bool basyn = true, bool bloadfromfile = true, bool bautoreleasebundle = true)
            {
                string[] spaths = new string[1];
                System.Type[] types = new System.Type[1];
                eLoadResPath[] eloadResTypes = new eLoadResPath[1];
                string[] stags = new string[1];
                spaths[0] = spath;
                types[0] = type;
                eloadResTypes[0] = eloadResType;
                stags[0] = tag;


                mpaths = spaths;
                mtypes = types;
                mproc = proc;
                mtags = stags;
                mo = o;
                mbasyn = basyn;
                mbautoreleasebundle = bautoreleasebundle;
                mbloadfromfile = bloadfromfile;
                meloadResTypes = eloadResTypes;
            }
            public CloadParam(string[] spaths, System.Type[] types, eLoadResPath[] eloadResTypes, string[] tags, ProcessDelegateArgc proc = null, object o = null, bool basyn = true, bool bloadfromfile = true, bool bautoreleasebundle = true)
            {
                mpaths = spaths;
                mtypes = types;
                meloadResTypes = eloadResTypes;
                mtags = tags;

                mproc = proc;
                mo = o;
                mbasyn = basyn;
                mbautoreleasebundle = bautoreleasebundle;
                mbloadfromfile = bloadfromfile;

            }
        }
        /// <summary>
        /// Cache中bundle信息
        /// </summary>
        internal class CacheBundleInfo : Singleton<CacheBundleInfo>
        {
            /// <summary>
            /// 是否已经初始化过
            /// </summary>
            private bool mbisInit = false;
            /// <summary>
            /// 内存中记录的cache信息
            /// </summary>
            private Dictionary<string, string> _mdicBundleInfo = new Dictionary<string, string>();
            /// <summary>
            /// cache文件路径所在的文件夹
            /// </summary>
            private string _smCachinginfofiledir = "";

            private CacheBundleInfo() { }
            /// <summary>
            /// 初始化cache
            /// </summary>
            public void initBundleInfo(string sCacheDir)
            {
                if (mbisInit == false)
                {
                    _smCachinginfofiledir = Application.persistentDataPath + "/bundles/" + sCacheDir;

                    string Cachinginfofile = _smCachinginfofiledir + "/cachinginfo.txt";
                    if (!Directory.Exists(_smCachinginfofiledir))
                    {
                        Directory.CreateDirectory(_smCachinginfofiledir);
                    }
                    FileStream fs = new FileStream(Cachinginfofile, FileMode.OpenOrCreate);
                    StreamReader sr = new StreamReader(fs);
                    string snum = sr.ReadLine();
                    int inum = 0;
                    if (int.TryParse(snum, out inum))
                    {
                        for (int i = 0; i < inum; i++)
                        {
                            string bundlepath = sr.ReadLine();
                            string shash = sr.ReadLine();
                            _mdicBundleInfo.Add(bundlepath, shash);
                        }
                    }
                    sr.Close();
                    fs.Close();
                    mbisInit = true;
                }

            }
            /// <summary>
            /// 将Cache信息写入硬盘
            /// </summary>
            public void saveBundleInfo()
            {
                string Cachinginfofile = _smCachinginfofiledir + "/cachinginfo.txt";
                StreamWriter sw = new StreamWriter(Cachinginfofile, false);
                List<string> listbundles = new List<string>(_mdicBundleInfo.Keys);
                sw.WriteLine(listbundles.Count);
                for (int i = 0; i < _mdicBundleInfo.Keys.Count; i++)
                {
                    sw.WriteLine(listbundles[i]);
                    sw.WriteLine(_mdicBundleInfo[listbundles[i]]);
                }
                sw.Flush();
                sw.Close();
            }
            /// <summary>
            /// 更新/增加内存 中的Cache信息
            /// </summary>
            /// <param name="path"></param>
            /// <param name="sMD5"></param>
            public void updateBundleInfo(string path, string sMD5)
            {
                if (_mdicBundleInfo.ContainsKey(path))
                {
                    _mdicBundleInfo[path] = sMD5;
                }
                else
                {
                    _mdicBundleInfo.Add(path, sMD5);
                }
            }
            /// <summary>
            /// cache中是否含有此bundle
            /// </summary>
            /// <param name="bundlepath"></param>
            /// <returns></returns>
            public bool hasBundle(string bundlepath)
            {
                return _mdicBundleInfo.ContainsKey(bundlepath);
            }
            /// <summary>
            /// cache中是否含有对应MD5的
            /// </summary>
            /// <param name="bundlepath"></param>
            /// <param name="sMD5"></param>
            /// <returns></returns>
            public bool isCaching(string bundlepath, string sMD5)
            {
                return _mdicBundleInfo.ContainsKey(bundlepath) && _mdicBundleInfo[bundlepath] == sMD5;
            }
            /// <summary>
            /// 删除内存cache中的bundle信息,并保存到硬盘
            /// </summary>
            /// <param name="bundlepath"></param>
            private void _deleteBundleInCaching(string bundlepath)
            {
                if (hasBundle(bundlepath))
                {
                    _mdicBundleInfo.Remove(bundlepath);
                    saveBundleInfo();
                    string scachingbundlepath = _smCachinginfofiledir + "/" + bundlepath;
                    if (File.Exists(scachingbundlepath))
                    {
                        File.Delete(scachingbundlepath);
                    }

                }
            }
            /// <summary>
            /// 清理cache里跟服务器上对应不上的bundle
            /// </summary>
            /// <param name="listbundlepaths"></param>
            public void clearNoUsedBundle(List<string> listURLBundlePaths)
            {
                List<string> listkeys = new List<string>(_mdicBundleInfo.Keys);
                for (int i = 0; i < listkeys.Count; i++)
                {
                    if (!listURLBundlePaths.Contains(listkeys[i]))
                    {//如果caching里面有,但是服务器上没有的bundle,则是无用的bundle,删除掉
                        _deleteBundleInCaching(listkeys[i]);
                    }
                }
            }
            
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
