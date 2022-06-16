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
using System.ComponentModel;

namespace WPFSTD105
{
    public class ExcelAmountService
    {
        /// <summary>
        /// 
        /// </summary>
        public ExcelAmountService()
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
                materialDataViews.
                   GroupBy(el1 => steelAttrs[steelAttrs.FindIndex(steel => steel.Profile == el1.Profile)].Type).
                        ForEach(obj =>
                        {
                            obj.
                                GroupBy(el2 => el2.Material).
                                    ForEach(mat =>
                                    {
                                        double sumAmo = 0, sumKg = 0, sumCount = 0;
                                        sheet.Cells[row, 0].Value=$"型鋼型態 : {obj.Key.ToString()}    材質 : {mat.Key}";

                                        sheet.Range[$"A{row+1}:G{row+1}"].Merge();
                                        sheet.Range[$"A{row+1}:G{row+1}"].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Range[$"A{row+1}:G{row+1}"].Borders.TopBorder.Color  =   Color.Black;
                                        row++;
                                        sheet.Cells[row, 0].Value = "購料長度";
                                        sheet.Cells[row, 1].Value = "數量";
                                        sheet.Cells[row, 2].Value = "斷面規格";
                                        sheet.Cells[row, 3].Value = "購料重量";
                                        sheet.Cells[row, 4].Value = "單價";
                                        sheet.Cells[row, 5].Value = "來源";
                                        sheet.Cells[row, 6].Value = "狀態";
                                        sheet.Cells[row, 0].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 1].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 2].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 3].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 4].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 5].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 6].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 0].Borders.TopBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 1].Borders.TopBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 2].Borders.TopBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 3].Borders.TopBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 4].Borders.TopBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 5].Borders.TopBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 6].Borders.TopBorder.Color  =   Color.Black;

                                        sheet.Cells[row, 0].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 1].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 2].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 3].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 4].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 5].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 6].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 0].Borders.RightBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 1].Borders.RightBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 2].Borders.RightBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 3].Borders.RightBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 4].Borders.RightBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 5].Borders.RightBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 6].Borders.RightBorder.Color  =   Color.Black;

                                        sheet.Cells[row, 0].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 1].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 2].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 3].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 4].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 5].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 6].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 0].Borders.LeftBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 1].Borders.LeftBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 2].Borders.LeftBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 3].Borders.LeftBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 4].Borders.LeftBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 5].Borders.LeftBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 6].Borders.LeftBorder.Color  =   Color.Black;

                                        sheet.Cells[row, 0].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 1].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 2].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 3].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 4].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 5].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 6].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                        sheet.Cells[row, 0].Borders.BottomBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 1].Borders.BottomBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 2].Borders.BottomBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 3].Borders.BottomBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 4].Borders.BottomBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 5].Borders.BottomBorder.Color  =   Color.Black;
                                        sheet.Cells[row, 6].Borders.BottomBorder.Color  =   Color.Black;
                                        row++;

                                        var pr = mat.GroupBy(el3 => el3.Profile);
                                        pr.OrderByDescending(el3 => el3.Key).
                                            ForEach(pro =>
                                            {
                                                double kg = steelAttrs[steelAttrs.FindIndex(steel => steel.Profile == pro.Key)].Kg;
                                                sheet.Cells[row, 2].Value = pro.Key;
                                                var len = pro.GroupBy(el4 => el4.LengthStr);
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Merge();
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.TopBorder.Color  =   Color.Black;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.LeftBorder.Color  =   Color.Black;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.RightBorder.Color  =   Color.Black;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Borders.BottomBorder.Color  =   Color.Black;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                                                sheet.Range[$"C{row+1}:C{row+len.Count()}"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                                                len.OrderByDescending(el4 => el4.Key).
                                                        ForEach(le =>
                                                        {
                                                            sheet.Cells[row, 0].Value = le.Key;
                                                            sheet.Cells[row, 1].Value = le.Count();
                                                            sheet.Cells[row, 3].Value =(kg * (le.Key/1000) * le.Count()).ToString("#0.00");
                                                            sheet.Cells[row, 4].Value = 29.3;
                                                            sheet.Cells[row, 5].Value = "";
                                                            sheet.Cells[row, 6].Value = "";
                                                            sumCount += le.Count();
                                                            sumKg += (kg * (le.Key/1000) * le.Count());
                                                            sumAmo += 29.3*sumKg;
                                                            sheet.Cells[row, 0].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 1].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 3].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 4].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 5].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 6].Borders.TopBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 0].Borders.TopBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 1].Borders.TopBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 3].Borders.TopBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 4].Borders.TopBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 5].Borders.TopBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 6].Borders.TopBorder.Color  =   Color.Black;

                                                            sheet.Cells[row, 0].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 1].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 3].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 4].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 5].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 6].Borders.LeftBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 0].Borders.LeftBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 1].Borders.LeftBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 3].Borders.LeftBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 4].Borders.LeftBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 5].Borders.LeftBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 6].Borders.LeftBorder.Color  =   Color.Black;

                                                            sheet.Cells[row, 0].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 1].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 3].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 4].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 5].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 6].Borders.RightBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 0].Borders.RightBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 1].Borders.RightBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 3].Borders.RightBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 4].Borders.RightBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 5].Borders.RightBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 6].Borders.RightBorder.Color  =   Color.Black;

                                                            sheet.Cells[row, 0].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 1].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 3].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 4].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 5].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 6].Borders.BottomBorder.LineStyle  =  BorderLineStyle.Thin;
                                                            sheet.Cells[row, 0].Borders.BottomBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 1].Borders.BottomBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 3].Borders.BottomBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 4].Borders.BottomBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 5].Borders.BottomBorder.Color  =   Color.Black;
                                                            sheet.Cells[row, 6].Borders.BottomBorder.Color  =   Color.Black;
                                                            row++;
                                                        });
                                            });
                                        sheet.Cells[row, 6].Value =$"預估金額 {(int)sumAmo} 元整";
                                        sheet.Cells[row, 3].Value =$" {sumKg.ToString("#0.00")} kg";
                                        sheet.Cells[row, 1].Value =$"合計 {sumCount} 支";
                                        row++;
                                    });
                        });

                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();
                book.BeginUpdate();
                book.SaveDocument(path, DocumentFormat.Xlsx);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
