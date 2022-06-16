using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WPFWindowsBase
{
    /// <summary>
    /// lambda 表達的助手
    /// </summary>
    public static class ExpressionHelpers
    {
        /// <summary>
        /// 編譯表達式並獲取函數的返回值
        /// </summary>
        /// <typeparam name="T">返回值的類型</typeparam>
        /// <param name="lambda">要編譯的表達式</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
        {
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// 將基礎屬性值設置為給定值
        /// 來自包含屬性的表達式
        /// </summary>
        /// <typeparam name="T">要設置的值的類型</typeparam>
        /// <param name="lambda">表達方式</param>
        /// <param name="value">將該屬性設置的值為</param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
        {
            // 轉換一個lambda () => some.Property, to some.Property
            var expression = (lambda as LambdaExpression).Body as MemberExpression;

            // 獲取屬性信息，以便我們進行設置
            var propertyInfo = (PropertyInfo)expression.Member;
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            // 設置屬性值
            propertyInfo.SetValue(target, value);

        }
    }
}
