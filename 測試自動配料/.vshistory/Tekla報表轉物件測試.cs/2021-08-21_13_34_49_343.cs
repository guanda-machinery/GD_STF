using NUnit.Framework;
using System;
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

        [Test]
        public void Test1()
        {
            new TeklaHtemlFactory(@"C:\Users\User\source\repos\GD_STF\測試自動配料\GUANDA_Personal_Format.xls").ToObject(typeof(SteelAttr), null, null);
            Assert.Pass();
        }
    }
}