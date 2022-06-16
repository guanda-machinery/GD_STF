using GD_STD.Attribute;
using GD_STD.MS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace STDTOMS_TEST
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MSIO()
        {
            try
            {
                MSIO ms = new MSIO();
                string tableName = ((MSTableAttribute)ms.GetType().GetCustomAttribute(typeof(MSTableAttribute))).Name;//表格名稱

                //取得所有屬性的附加屬性
                var mSFieldAttribute = GetMsFieldAttribute(ms);

                var field = mSFieldAttribute.Select(el => el.Name).Aggregate((el, ex) => $"{el},{ex}"); //資料表欄位名稱

               
                string parName = mSFieldAttribute.Select(el => el.Description).Aggregate((el, ex) => $"{el},{ex}"); //參數名稱

                string sql = $"INSERT into {tableName} ({field}) VALUES ({parName})"; //參數名稱

                //Assert.AreEqual(mSFieldAttribute.Select(el => el.Description).Aggregate((el, ex) => $"{el},{ex}"), field);
                //return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
           
        }
        /// <summary>
        /// 取得 <see cref="AbsMS"/> 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private static List<MSFieldAttribute> GetMsFieldAttribute(AbsMS ms) => ms.AllValue()
            .Where((e) => e.GetCustomAttribute(typeof(MSFieldAttribute)) != null)
            .Select(el => (MSFieldAttribute)el.GetCustomAttribute(typeof(MSFieldAttribute))).ToList();
    }
}
