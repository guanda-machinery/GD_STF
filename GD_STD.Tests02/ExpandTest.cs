// <copyright file="ExpandTest.cs">Copyright ©  2021</copyright>
using System;
using System.Reflection;
using GD_STD;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GD_STD.Tests
{
    /// <summary>此類別包含 Expand 的參數化單元測試</summary>
    [PexClass(typeof(Expand))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ExpandTest
    {
        /// <summary>GetValue(MemberInfo, Object) 的測試虛設常式</summary>
        [PexMethod]
        public object GetValueTest(MemberInfo info, object data)
        {
            object result = Expand.GetValue(info, data);
            return result;
            // TODO: 將判斷提示加入 方法 ExpandTest.GetValueTest(MemberInfo, Object)
        }

        /// <summary>Reflection(Object, Type) 的測試虛設常式</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public TResult ReflectionTest<TResult>(object value, Type type)
        {
            TResult result = Expand.Reflection<TResult>(value, type);
            return result;
            // TODO: 將判斷提示加入 方法 ExpandTest.ReflectionTest(Object, Type)
        }

        /// <summary>SetValue(MemberInfo, Object, Object) 的測試虛設常式</summary>
        [PexMethod]
        public void SetValueTest(
            MemberInfo info,
            object data,
            object setValue
        )
        {
            Expand.SetValue(info, data, setValue);
            // TODO: 將判斷提示加入 方法 ExpandTest.SetValueTest(MemberInfo, Object, Object)
        }
    }
}
