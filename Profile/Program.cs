using GD_STD;
using System.Collections.ObjectModel;
using System.IO;
using WPFSTD105.Attribute;
using System;
using WPFSTD105;
using GD_STD.Enum;

namespace Profile
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int counter = 0;
                string line;
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(Console.ReadLine());
                ObservableCollection<SteelAttr> steels = new ObservableCollection<SteelAttr>();
                OBJETC_TYPE oBJETC_TYPE = (OBJETC_TYPE)Convert.ToInt16(Console.ReadLine());
                while ((line = file.ReadLine()) != null)
                {
                    string[] vs = line.Split(',');
                    steels.Add(new SteelAttr()
                    {
                        Profile = vs[0],
                        H = Convert.ToSingle(vs[1]),
                        W = Convert.ToSingle(vs[2]),
                        t1 = Convert.ToSingle(vs[3]),
                        t2 = Convert.ToSingle(vs[4]),
                        Kg = Convert.ToSingle(vs[5]),
                        Type = oBJETC_TYPE
                    });
                    counter++;
                }
                SerializationHelper.SerializeBinary(steels, Console.ReadLine());
            }

        }
    }
}
