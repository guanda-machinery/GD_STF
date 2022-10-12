using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using GD_STD;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFWindowsBase;
using DevExpress.Data.Extensions;
namespace WPFSTD105
{
    public class ExcelBuyService
    {
        /// <summary>
        /// 
        /// </summary>
        public ExcelBuyService()
        {
        }
        #region 私有屬性
        #endregion
        /// <summary>
        /// 創建文件
        /// </summary>
        /// <param name="path">儲存路徑</param>
        public void CreateFile(string path, ObservableCollection<MaterialDataView> dataViews, ObservableCollection<SteelAttr> steelAttrs = null)
        {
            SpreadsheetControl spreadSheet = new SpreadsheetControl();
            try
            {
                int row = 0;
                IWorkbook book = spreadSheet.Document; //提供對控件中加載的工作簿的訪問
                Worksheet sheet = book.Worksheets.Add(); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                ReadOnlyCollection<MaterialDataView> materialDataViews = new ReadOnlyCollection<MaterialDataView>(dataViews);
                steelAttrs = steelAttrs == null ? SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\RH.inp") : steelAttrs;
                materialDataViews.GroupBy(el => el.Profile).ForEach(el =>
                {
                    sheet.Cells[row, 0].Value=$"斷面規格 : {el.Key}";
                    row++;
                    sheet.Range[$"A{row}:H{row}"].Merge();
                    sheet.Cells[row, 0].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                    sheet.Cells[row, 0].Borders.TopBorder.Color  =   Color.Black;
                    sheet.Cells[row, 0].Value = "購料長度";
                    sheet.Cells[row, 1].Value = "切割長度組合";
                    sheet.Cells[row, 2].Value = "材質";
                    sheet.Cells[row, 3].Value = "加工長度";
                    sheet.Cells[row, 4].Value = "損耗";
                    sheet.Cells[row, 5].Value = "購料重量";
                    sheet.Cells[row, 6].Value = "來源";
                    sheet.Cells[row, 7].Value = "狀態";

                    {
                        sheet.Cells[row, 0].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.TopBorder.Color  =   Color.Black;

                        sheet.Cells[row, 0].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.RightBorder.Color  =   Color.Black;

                        sheet.Cells[row, 0].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.LeftBorder.Color  =   Color.Black;

                        sheet.Cells[row, 0].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.BottomBorder.Color  =   Color.Black;
                    }
                   
                    row++;
                    foreach (var item in el)
                    {
                        int index = steelAttrs.FindIndex(steel => steel.Profile == item.Profile);
                        sheet.Cells[row, 0].Value = item.LengthStr;
                        sheet.Cells[row, 1].Value = item.Parts.Select(part => $"{part.Length}").Aggregate((str1, str2) => $"{str1},{str2}");
                        sheet.Cells[row, 2].Value = item.Parts[0].Material;
                        sheet.Cells[row, 3].Value = item.Loss;
                        sheet.Cells[row, 4].Value =  item.LengthStr - item.Loss;
                        sheet.Cells[row, 5].Value =( index == -1 ? 0 : item.LengthStr / 1000 * steelAttrs[index].Kg).ToString("#0.00");
                        sheet.Cells[row, 6].Value = item.Sources;
                        sheet.Cells[row, 0].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.TopBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.TopBorder.Color  =   Color.Black;

                        sheet.Cells[row, 0].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.RightBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.RightBorder.Color  =   Color.Black;

                        sheet.Cells[row, 0].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.LeftBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.LeftBorder.Color  =   Color.Black;

                        sheet.Cells[row, 0].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 1].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 2].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 3].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 4].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 5].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 6].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 7].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                        sheet.Cells[row, 0].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 1].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 2].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 3].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 4].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 5].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 6].Borders.BottomBorder.Color  =   Color.Black;
                        sheet.Cells[row, 7].Borders.BottomBorder.Color  =   Color.Black;
                        row++;
                    }
                });
                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                book.BeginUpdate();
                book.SaveDocument(path, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
