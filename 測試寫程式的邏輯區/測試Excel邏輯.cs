using GD_STD.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Spreadsheet;
using WPFWindowsBase;
using System.Threading;
using DevExpress.XtraSpreadsheet.Export.Xls;
using System.Data;
using System.Reflection;
using System.Drawing;
using DevExpress.Xpf.Editors;
using ZXing;
using ZXing.QrCode;
using WPFSTD105.Attribute;
using GD_STD;

namespace 測試寫程式的邏輯區
{
    [TestFixture]
    public class 測試Excel邏輯
    {
        [Test]
        public void CreateFileCut()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                ExcelCutService excelService = new ExcelCutService();
                STDSerialization ser = new STDSerialization();
                ObservableCollection<MaterialDataView> _ = ser.GetMaterialDataView(@"C:\Users\User\source\repos\GD_STF\GD_STF\測試寫程式的邏輯區\MDV.lis");
                excelService.CreateFile(@"C:\Users\User\Desktop\TESddd.xls", _);
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            thread.Abort();
        }
        [Test]
        public void CreateFileBuy()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                ExcelBuyService excelService = new ExcelBuyService();
                STDSerialization ser = new STDSerialization();
                ObservableCollection<MaterialDataView> _ = ser.GetMaterialDataView(@"C:\Users\User\source\repos\GD_STF\GD_STF\測試寫程式的邏輯區\MDV.lis");
                ObservableCollection<SteelAttr> steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\Desktop\Model\0507-2世紀代工-最大直\Profile\RH.inp");
                excelService.CreateFile(@"C:\Users\User\Desktop\buy.xls", _, steelAttrs);
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            thread.Abort();
            
        }
        [Test]
        public void CreateFileAmount()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                ExcelAmountService excelService = new ExcelAmountService();
                STDSerialization ser = new STDSerialization();
                ObservableCollection<MaterialDataView> _ = ser.GetMaterialDataView(@"C:\Users\User\source\repos\GD_STF\GD_STF\測試寫程式的邏輯區\MDV.lis");
                ObservableCollection<SteelAttr> steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"C:\Users\User\Desktop\Model\0507-2世紀代工-最大直\Profile\RH.inp");
                excelService.CreateFile(@"C:\Users\User\Desktop\Amount.xls", _, steelAttrs);
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            thread.Abort();
        }
    }
    /// <summary>
    /// 表單
    /// </summary>
    public interface IWorksheet
    {
        /// <summary>
        /// 標題
        /// </summary>
        object[] Title { get; set; }

    }

    public class MyWeatherConverter : IBindingRangeValueConverter
    {
        public object ConvertToObject(CellValue value, Type requiredType, int columnIndex)
        {
            return null;
        }

        public CellValue TryConvertFromObject(object value)
        {
            return null;
        }
    }
}
