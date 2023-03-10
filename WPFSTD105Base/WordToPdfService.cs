using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace WordToPdfService
{
    public class PdfConverter : IDisposable
    {
        private Application wordApp = null;

        public PdfConverter()
        {
            wordApp = new Application();
            wordApp.Visible = false;
        }

        public byte[] GetPdf(string templateFile)
        {
            object filePath = templateFile;
            //檔案先寫入系統暫存目錄
            object outFile =
                Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
            Document doc = null;
            try
            {
                object readOnly = true;
                doc = wordApp.Documents.Open(FileName: ref filePath, ReadOnly: ref readOnly);
                //doc.Activate();
                //存成PDF檔案
                object fileFormat = WdSaveFormat.wdFormatPDF;
                doc.SaveAs2(FileName: ref outFile, FileFormat: ref fileFormat);
                //關閉Word檔
                object dontSave = WdSaveOptions.wdDoNotSaveChanges;
                ((_Document)doc).Close(ref dontSave);
            }
            finally
            {
                //確保Document COM+釋放
                if (doc != null)
                    Marshal.FinalReleaseComObject(doc);
                doc = null;
            }
            //讀取PDF檔，並將暫存檔刪除
            byte[] buff = File.ReadAllBytes(outFile.ToString());
            File.Delete(outFile.ToString());
            return buff;
        }

        public void Dispose()
        {
            //確實關閉Word Application
            try
            {
                object dontSave = WdSaveOptions.wdDoNotSaveChanges;
                ((_Application)wordApp).Quit(ref dontSave);
            }
            finally
            {
                Marshal.FinalReleaseComObject(wordApp);
            }
        }
    }
}