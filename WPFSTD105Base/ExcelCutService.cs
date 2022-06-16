using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;
using ZXing;
using ZXing.QrCode;

namespace WPFSTD105
{
    public class ExcelCutService
    {
        /// <summary>
        /// 
        /// </summary>
        public ExcelCutService()
        {

        }
        #region 私有屬性

        #endregion
        /// <summary>
        /// 創建文件
        /// </summary>
        /// <param name="path">儲存路徑</param>
        public void CreateFile(string path, ObservableCollection<MaterialDataView> dataViews)
        {
            SpreadsheetControl spreadSheet = new SpreadsheetControl();
            try
            {
                int row = 0;
                IWorkbook book = spreadSheet.Document; //提供對控件中加載的工作簿的訪問
                Worksheet sheet = book.Worksheets.Add(); //創建一個新工作表並將其附加到集合的末尾
                book.Worksheets.ActiveWorksheet = sheet;
                ReadOnlyCollection<MaterialDataView> materialDataViews = new ReadOnlyCollection<MaterialDataView>(dataViews);
                Type viewType = typeof(MaterialDataView);
                var pView = viewType.GetProperties().Where(el => el.GetCustomAttribute<ExcelAttribute>()!= null).ToList();
                pView.Sort((p1, p2) =>
                {
                    int pp2 = p2.GetCustomAttribute<ExcelAttribute>().Index;
                    int pp1 = p1.GetCustomAttribute<ExcelAttribute>().Index;
                    return pp1.CompareTo(pp2);
                });
                sheet.Cells[row, pView.Count].Value = "狀態";
                sheet.Cells[row, pView.Count].Font.Size = 14;
                pView.ForEach(p =>
                {
                    var att = p.GetCustomAttribute<ExcelAttribute>();
                    sheet.Cells[row, att.Index].Value = att.ColumnName;
                    sheet.Cells[row, att.Index].Font.Size = 14;
                });
                row++;
                Type partType = typeof(TypeSettingDataView);
                var pPart = partType.GetProperties().Where(el => el.GetCustomAttribute<ExcelAttribute>() != null).ToList();
                dataViews.ForEach(view =>
                {
                    pView.ForEach(el =>
                    {
                        ExcelAttribute att = el.GetCustomAttribute<ExcelAttribute>();

                        sheet.Cells[row, att.Index].Value = el.GetValue(view)?.ToString();
                        sheet.Cells[row, att.Index].Font.Size = 14;

                        sheet.Cells[row, att.Index].Borders.TopBorder.LineStyle =  BorderLineStyle.Thin;
                        sheet.Cells[row, att.Index].Borders.TopBorder.Color = Color.Black;
                    });
                    sheet.Cells[row, pView.Count].Borders.TopBorder.LineStyle =  BorderLineStyle.Thin;
                    sheet.Cells[row, pView.Count].Borders.TopBorder.Color = Color.Black;
                    row++;
                    pPart.ForEach(el =>
                    {
                        ExcelAttribute att = el.GetCustomAttribute<ExcelAttribute>();
                        sheet.Cells[row, att.Index].Value =att.ColumnName;
                        sheet.Cells[row, att.Index].Font.Size = 14;
                    });
                    row++;
                    int startRow = row;
                    view.Parts.ForEach(part =>
                    {
                        pPart.ForEach(el =>
                        {
                            ExcelAttribute att = el.GetCustomAttribute<ExcelAttribute>();
                            sheet.Cells[row, att.Index].Value = el.GetValue(part).ToString();
                            sheet.Cells[row, att.Index].Font.Size = 14;
                        });
                        BarcodeWriter _writer = new BarcodeWriter();
                        _writer.Format = BarcodeFormat.CODABAR;
                        QrCodeEncodingOptions _qrCodeEncoding = new QrCodeEncodingOptions()
                        {
                            DisableECI = true,//設定內容編碼
                            CharacterSet = "UTF-8", //設定二維碼的寬度和高度
                            Width =80,
                            Height = 15,
                            Margin = 0,//設定二維碼的邊距,單位不是固定畫素
                        };

                        _writer.Options = _qrCodeEncoding;
                        Bitmap _map = _writer.Write(Convert.ToInt64(part.Length).ToString());

                        sheet.Pictures.AddPicture(_map, sheet.Cells[row, pPart.Count+1]);

                        row++;
                    });
                    sheet.Range[$"A{startRow}:A{row}"].Merge();

                    BarcodeWriter writer = new BarcodeWriter();
                    writer.Format = BarcodeFormat.QR_CODE;
                    QrCodeEncodingOptions qrCodeEncoding = new QrCodeEncodingOptions()
                    {
                        DisableECI = true,//設定內容編碼
                        CharacterSet = "UTF-8", //設定二維碼的寬度和高度
                        Width =60,
                        Height = 60,
                        Margin = 1//設定二維碼的邊距,單位不是固定畫素
                    };

                    writer.Options = qrCodeEncoding;
                    Bitmap map = writer.Write(view.MaterialNumber);

                    sheet.Pictures.AddPicture(map, sheet.Cells[startRow-1, 0]);
                });

                sheet.Cells.AutoFitColumns();
                sheet.Rows.AutoOutline();

                //var binding = sheet.DataBindings.BindTableToDataSource(table, 1, 1);
                //object _ = binding.DataSource;
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
