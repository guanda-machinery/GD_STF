using GD_STD.Attribute;
using GD_STD.MS;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace STDTOMS
{
    /// <summary>
    /// MSSQL 幫手
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class MssqlHelper<T> where T : AbsMS
    {
        /// <summary>
        /// 連線字段
        /// </summary>
        private const string Constr =
            @"Data Source=2012R2\SQLEXPRESS;Initial Catalog=三軸;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// 插入資料行
        /// </summary>
        /// <param name="ms"><see cref="IMS"/></param>
        /// <example>
        ///  此示例顯示瞭如何調用 <see cref="Insert(T)"/> 方法
        /// <code>
        /// class Program
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         //回傳整個附加屬性
        ///         var ms = new MSIO();
        ///         ms.Field1 = "欄位1內容";
        ///         ms.Field2 = "欄位2內容";
        ///         MssqlHelper&lt;MSIO&gt;.Insert(ms);
        ///     }
        /// }
        /// [AbsCustomAttribute("資料表名稱", "myClass Description")]
        /// public sealed class MSIO : AbsMS
        /// {
        ///     [AbsCustomAttribute("欄位1", "欄位1參數符號")]
        ///     public string Field1 { get; set; }
        ///     [AbsCustomAttribute("欄位2", "欄位2參數符號")]
        ///     public string Field2 { get; set; }
        /// }
        /// 
        /// public class AbsCustomAttribute : System.Attribute
        /// {
        ///     public AbsCustomAttribute(string name, string description)
        ///     {
        ///         Name = name;
        ///         Description = description;
        ///     }
        ///     public string Name { get; protected set; }
        ///     public string Description { get; protected set; }
        /// }
        /// </code>
        /// </example>
        public static void Insert(T ms)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Constr))
            {
                string tableName = ((MSTableAttribute)ms.GetType().GetCustomAttribute(typeof(MSTableAttribute))).Name;//表格名稱

                //取得所有屬性的附加屬性
                var mSFieldAttribute = GetMsFieldAttribute(ms);

                var field = mSFieldAttribute.Select(el => el.Name).Aggregate((el, ex) => $"{el},{ex}"); //資料表欄位名稱

                string parName = mSFieldAttribute.Select(el => el.Description).Aggregate((el, ex) => $"{el},{ex}"); //參數名稱

                string sql = $"INSERT into {tableName} ({field}) VALUES ({parName})"; //參數名稱
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    PropertyInfo[] value = ms.AllValue();
                    IList<MSFieldAttribute> mS = mSFieldAttribute.ToList();
                    for (int i = 0; i < value.Length; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(mS[i].Description, value[i].GetValue(ms));
                    }
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Clone();
                }
            }
        }
        /// <summary>
        /// 取得 <see cref="AbsMS"/> 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private static List<MSFieldAttribute> GetMsFieldAttribute(T ms) => ms.AllValue()
                    .Where((e) => e.GetCustomAttribute(typeof(MSFieldAttribute)) != null)
                    .Select(el => (MSFieldAttribute)el.GetCustomAttribute(typeof(MSFieldAttribute))).ToList();
    }
}