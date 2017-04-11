using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;
using PSupport;
/*******************************************************************************
* 
*             类名: FastCSVFile
*             功能: 快速读取CSV格式,支持字段内含有逗号和引号的读取,减少读取时的对内存分配操作
*             作者: 彭谦
*             日期: 2016.11.28
*             修改:
*            
*             
* *****************************************************************************/
namespace Config
{

    internal enum EQuotState
    {
        //没有引号
        eQuot_Zero = 0,
        //引号奇数
        eQuot_Odd  = 1,
        //引号偶数
        eQuot_Even = 2
    }
    /// <summary>
    /// 读取CSV文件类
    /// </summary>
    public class FastCSVFile
    {
        #region 成员变量
        private string _msfilename = "";
        /// <summary>
        /// 记录数据流
        /// </summary>
        private byte[] _mbytes = null;

        /// <summary>
        /// 记录目前读取bytes的位置
        /// </summary>
        private int _miCurIndex = 0;

        /// <summary>
        /// 用来做字符串操作的StringBuilder
        /// </summary>
        private static StringBuilder _msb = new StringBuilder();
        
        /// <summary>
        /// 获取CSV行数据
        /// </summary>
        public  FastCSVRow mCsvRow = new FastCSVRow();
        #endregion
        /// <summary>
        /// 用bytes初始化类
        /// </summary>
        /// <param name="bytes"></param>
        public FastCSVFile(string sfilename, byte[] bytes)
        {
            _msfilename = sfilename;
            _mbytes = bytes;
        }
        #region 读取行数据
        /// <summary>
        /// 用来读取csv的行，返回false为读取完毕
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool readRow()
        {
            //如果超过字节数,则为读取完毕
            if (_miCurIndex >= _mbytes.Length - 1)
            {
                mCsvRow.Clear();
                mCsvRow._mbEnd = true;
                return !mCsvRow.isEnd;
            }
            //行数加一
            mCsvRow._mRowNum++;
            //清空连接字符
            _msb.Remove(0, _msb.Length);
            //清空用来存储分割字符串的list
            mCsvRow.Clear();
            //记录一个分隔段的起点index
            int istatindex = -1;
            //记录一个分隔段的长度
            int ilength = 0;
            //是否换行
            bool botherline = false;
            //引号状态
            EQuotState equotstate = EQuotState.eQuot_Zero;

            //这里逐个字符的判断
            while (botherline == false)
            {
                
                int indexadd = 1;
                if (_miCurIndex >= _mbytes.Length - 1)
                {//如果超出,即没有换行符但是是最后一行
                    _miCurIndex = _mbytes.Length - 1;
                    ilength = _mbytes.Length - istatindex;
                    botherline = true;
                }
                //如果是换行符,则退出
                if (_mbytes[_miCurIndex] == '\r'
                    && _miCurIndex + 1 < _mbytes.Length
                    && _mbytes[_miCurIndex + 1] == '\n')
                {// \r\n的情况
                    indexadd = 2;
                    botherline = true;
                }
                else if (_mbytes[_miCurIndex] == '\r' || _mbytes[_miCurIndex] == '\n')
                {// \r or \n 的情况
                    botherline = true;
                }
                
                if ((_mbytes[_miCurIndex] == ',' 
                        && (equotstate == EQuotState.eQuot_Even 
                        || equotstate == EQuotState.eQuot_Zero)) 
                        || botherline)
                {//如果是逗号,并且逗号记录没有,或者为偶数,或者为换行符,则可以解析分隔一段字符串
                    string str = string.Empty;
                    if (ilength != 0)
                    {
                        str = System.Text.Encoding.UTF8.GetString(_mbytes, istatindex, ilength);
                    }
                    if (equotstate == EQuotState.eQuot_Zero)
                    {//如果不是引号内
                        mCsvRow.Add(str);
                    }
                    else
                    {
                        _msb.Append(str);
                        //去掉头尾引号,将2个连续引号替换为1个引号
                        _msb.Remove(0, 1); ;
                        _msb.Remove(_msb.Length - 1,1);
                        _msb.Replace("\"\"", "\"");
                        mCsvRow.Add(_msb.ToString());
                        _msb.Remove(0, _msb.Length);
                        equotstate = EQuotState.eQuot_Zero;
                    }
                    ilength = 0;
                    istatindex = -1;
                }
                else if (_mbytes[_miCurIndex] == '"')
                {//如果是引号,则设置引号状态
                    if (equotstate == EQuotState.eQuot_Zero || equotstate == EQuotState.eQuot_Even)
                    {
                        equotstate = EQuotState.eQuot_Odd;
                    }
                    else
                    {
                        equotstate = EQuotState.eQuot_Even;
                    }
                    ilength++;
                    if (istatindex == -1)
                    {
                        istatindex = _miCurIndex;
                    }
                }
                else
                {//如果是普通字符 或者引号内的字符
                    ilength++;
                    if (istatindex == -1)
                    {
                        istatindex = _miCurIndex;
                    }

                }
                _miCurIndex += indexadd;
            }
            return !mCsvRow.isEnd;
        }
        #endregion
        /// <summary>
        /// 重置
        /// </summary>
        public void reset()
        {
            _miCurIndex = 0;
            mCsvRow._mRowNum = -1;
        }
        #region 转换接口
        //类型转换接口
        public string getString(int index)
        {
            try
            {
                return mCsvRow[index];
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return "";
            }
        }
        public int getInt(int index)
        {
            try
            {
                return Convert.ToInt32(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return 0;
            }
        }

        public Int16 getInt16(int index)
        {
            try
            {
                return Convert.ToInt16(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return 0;
            }
        }

        public byte getInt8(int index)
        {
            try
            {
                return Convert.ToByte(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return 0;
            }
        }

        public UInt16 getUInt16(int index)
        {
            try
            {
                return Convert.ToUInt16(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return 0;
            }
        }

        public bool getBool(int index)
        {
            try
            {
                return Convert.ToBoolean(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return false;
            }
        }

        public UInt32 getUInt(int index)
        {
            try
            {
                return Convert.ToUInt32(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return 0;
            }
        }
        public float getFloat(int index)
        {
            try
            {
                return Convert.ToSingle(mCsvRow[index]);
            }
            catch (Exception e)
            {
                DLoger.LogError(_msfilename + "==csv row convert error at row:" + mCsvRow.CurrentRow + "=col:" + index);
                DLoger.LogError(e);
                return 0.0f;
            }
        }
        #endregion
    }
    #region CSV行类型
    /// <summary>
    /// CSV行类
    /// </summary>
    public class FastCSVRow : List<string>
    {
        /// <summary>
        /// 是否是结束行
        /// </summary>
        internal bool _mbEnd = false;
        
        /// <summary>
        /// 获取是否是结束行
        /// </summary>
        public bool isEnd { get { return _mbEnd; } }

        /// <summary>
        /// 行数
        /// </summary>
        internal int _mRowNum = -1;
        /// <summary>
        /// 返回目前行数
        /// </summary>
        public int CurrentRow { get { return _mRowNum; } }
        /// <summary>
        /// 返回当前行的列数
        /// </summary>
        public int ColNum { get { return this.Count; } }
    }
    #endregion
}

