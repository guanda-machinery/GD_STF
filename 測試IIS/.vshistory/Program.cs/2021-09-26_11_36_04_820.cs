﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 測試IIS.ExternalImport;

namespace 測試IIS
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
    class ImportCallbackHandler : ExternalImport.IExternalImportCallback
    {
        /// <summary>
        /// 比對衝衝突
        /// </summary>
        /// <param name="list"></param>
        public void Conflict(SteelPart[] list)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 結束寫入資料流
        /// </summary>
        public void EndFile()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///  資料夾路徑
        /// </summary>
        /// <param name="path">回傳路徑</param>
        public void ResponseDirectory(string[] path)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 寫入資料流位置
        /// </summary>
        /// <param name="position"></param>
        /// <param name="schedule"></param>
        public void WriteStream(long position, long schedule)
        {
            throw new NotImplementedException();
        }
    }
}
