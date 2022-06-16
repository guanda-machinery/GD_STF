using System;
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

        public void EndFile()
        {
            throw new NotImplementedException();
        }

        public void ResponseDirectory(string[] path)
        {
            throw new NotImplementedException();
        }

        public void WriteStream(int position, int schedule)
        {
            throw new NotImplementedException();
        }
    }
}
