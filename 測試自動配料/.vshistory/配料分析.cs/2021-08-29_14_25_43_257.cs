using GD_STD;
using GD_STD.Data;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;

namespace 測試自動配料
{
    [TestFixture]
    public class 配料分析
    {
        private static IEnumerable<string> GetBom()
        {
            //IoC.Setup();
            yield return "昇鋼金屬有限公司";
            yield return "卜蜂";
            yield return "南亞科技";
            yield return "英發";
            yield return "高雄港";
            yield return "觀音運動中心";
        }
        /// <summary>
        /// 每一個測試單元都會進入這方法
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }
        /// <summary>
        /// 單完測試結束
        /// </summary>
        [TearDown]
        public void Cleanup()
        {

        }
        [Test, TestCaseSource("GetBom")]
        public void 自動配料(string projectName)
        {
            ApplicationViewModel.ProjectName = projectName;

        }
        [Test, TestCaseSource("GetBom")]
        public void 配對料單設定(string projectName)
        {
            ApplicationViewModel.ProjectName = projectName;


        }
    }
}
