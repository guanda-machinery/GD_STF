using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace WPFSTD105
{
    internal static class WR_CommaSeparatedValues
    {
        public static bool ReadCSV(string FilePath, out List<object> ReadData, bool _hasHeaderRecord = true)
        {
            ReadData = new List<object>();
            try
            {
                var readConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = _hasHeaderRecord
                };

                using (var reader = new StreamReader(FilePath))
                using (var csv = new CsvReader(reader, readConfiguration))
                {
                    ReadData = csv.GetRecords<object>().ToList();
                }
                return true;
            }
            catch
            {
            }
            return false;
        }
        public static bool WriteCSV(string FilePath,  List<object> WriteData, bool _hasHeaderRecord = true)
        {
            try
            {
                var writeConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = _hasHeaderRecord
                };

                using (var writer = new StreamWriter(FilePath))
                using (var csv = new CsvWriter(writer, writeConfiguration))
                {
                    csv.WriteRecords(WriteData);
                }

                return true;
            }
            catch
            {
            }
            return false;
        }


    }
}
