using NUnit.Framework;
using System.Collections.Generic;
using WPFSTD105.Attribute;
using WPFSTD105.Tekla;
namespace 測試自動配料
{
    [TestFixture]
    public class Tests
    {
        private IEnumerable<List<object>> SteelAttrs()
        {
            yield return new TeklaHtemlFactory("GUANDA_Personal_Format.xls").ToObject(typeof(SteelAttr),null, null);
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