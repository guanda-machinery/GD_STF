using NUnit.Framework;
using System.Collections.Generic;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
namespace 測試自動配料
{
    public class Tests
    {
        private List<SteelAttr> SteelAttrs()
        {

            return null;
        }
        [SetUp]
        public void Setup()
        {

        }

        [Test, TestCaseSource("SteelAttrs")]
        public void Test1(List<SteelAttr> SteelAttrs)
        {
            Assert.Pass();
        }
    }
}